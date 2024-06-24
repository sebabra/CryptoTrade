using AutoMapper;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoTrade.Binders;
using CryptoTrade.Entities;
using CryptoTrade.Helpers;
using CryptoTrade.Repositories;
using CryptoTrade.Service.Binance;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;


namespace CryptoTrade.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CandlestickController : ControllerBase
{

    private readonly BinanceRestClient _binanceRestClient;
    private readonly IMapper _mapper;
    private readonly ICandlestickRepository _candlestickRepository;
    private readonly IIntervalRepository _intervalRepository;
    private readonly ISymbolRepository _symbolRepository;
    private readonly ILogger<CandlestickController> _logger;
    private readonly ExchangeData _exchangeData;

    public CandlestickController(BinanceRestClient binanceRestClient,
                                 IMapper mapper,
                                 ICandlestickRepository candlestickRepository,
                                 IIntervalRepository intervalRepository,
                                 ISymbolRepository symbolRepository,
                                 ILogger<CandlestickController> logger,
                                 ExchangeData exchangeData)
    {
        _binanceRestClient = binanceRestClient;
        _mapper = mapper;
        _candlestickRepository = candlestickRepository;
        _intervalRepository = intervalRepository;
        _logger = logger;
        _symbolRepository = symbolRepository;
        _exchangeData = exchangeData;
    }



    // TO DO : Vérifier que les appelles a l'api binance ne dépasse pas le MAX
    // TO DO : Faire + des appells a binance en parallele ?
    [HttpGet]
    public async Task<ActionResult> getCandlesticks([FromQuery] string symbol,
        [FromQuery, ModelBinder(BinderType = typeof(KlineIntervalBinder))] KlineInterval interval,
        [FromQuery] DateTime startTime,
        [FromQuery] DateTime endTime)
    {
        #region Check Start time + end time valide par rapport a l'interval demandé
        var intervalHelper = new IntervalHelper(interval);
        try
        {
            if (!intervalHelper.isValidStartTime(startTime, out DateTime correctedStartTime))
            {
                return BadRequest($"La startTime {startTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")} n'est pas valid pour l'interval {interval}.\n" +
                                  $"La plus proche startTime valide en dessous est {correctedStartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");
            }
            if (!intervalHelper.isValidEndTime(endTime, out DateTime correctedEndTime))
            {
                return BadRequest($"La endTime {endTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")} n'est pas valide pour l'intervalle {interval}.\n" +
                                  $"La plus proche endTime valide au dessus est {correctedEndTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");
            }
        }
        catch (Exception error)
        {
            return BadRequest(error.Message);
        }
        #endregion

        #region Check si end time est > start time
        if (endTime < startTime)
        {
            return BadRequest($"La end time {endTime} ne peut pas être < a la startTime {startTime}");
        }
        #endregion

        #region Check end time < Date Time actuelle et start time < Date actuelle
        DateTime now = DateTime.Now.ToUniversalTime();
        if (endTime >= now)
        {
            return BadRequest($"La endTime {endTime} ne peux pas être >= a maintenant {now}");
        }
        if (startTime >= now)
        {
            return BadRequest($"La startTime {startTime} ne peux pas être >= a maintenant {now}");
        }
        #endregion

        #region Check startTime et endTime + 1 ms sont bien entier
        var nbTotalSecondsRequested = (endTime.AddMilliseconds(1) - startTime).TotalSeconds;
        if (!(nbTotalSecondsRequested == Math.Floor(nbTotalSecondsRequested))) return BadRequest($"The start time {startTime} and end time {endTime} did not return an integer when substrating them together");
        #endregion

        #region Get the interval from the DB
        var dbInterval = await _intervalRepository.GetInterval((int)interval);
        if (dbInterval == null) return NotFound($"Interval {interval} does not exist in the DB");
        #endregion

        #region Get the Symbol from the DB
        var dbSymbol = await _symbolRepository.GetSymbolByName(symbol);
        if (dbSymbol == null)
        {
            // Si symbol null il faut aller voir dans binance si le symbol existe bien 
            // Si oui on le crée
            var binanceSymbol = await _exchangeData.getSymbolInformation(symbol);
            if (binanceSymbol == null) return NotFound($"Symbol {symbol} does not exist in binance");
            // Créer le symbol en DB
            dbSymbol = _mapper.Map<Symbol>(binanceSymbol);
            await _symbolRepository.SaveSymbol(dbSymbol);
        }
        #endregion


        // Get Candles that are in the DB
        var dbCandlesticks = await _candlestickRepository.GetCandlesticks(startTime, endTime, (int)interval, symbol);
        // Convertir l'entity vers le dto. Toujours travailler sur le DTO
        var dbCandlesticksDto = _mapper.Map<IEnumerable<CandlestickDto>>(dbCandlesticks);

        // Check si des gaps sont sur les candle récupérer de la DB
        var gaps = Gap.analyseGap(dbCandlesticksDto, startTime, endTime);

        #region Get les candles des gaps depuis l'api de binance
        var binanceCandlesticksDto = new List<CandlestickDto>();
        // Il faut seulement demander a binance les candles ou il y a des gaps
        foreach (Gap gap in gaps)
        {
            try
            {
                var binanceCandles = await getBinanceCandlesticks(symbol, interval, gap);

                #region Check si les candles recu de binance se suivent bien
                try
                {
                    CandlestickHelper.isContinuous(binanceCandles);
                }
                catch (Exception error)
                {
                    BadRequest(error.Message);
                }
                #endregion

                binanceCandlesticksDto.AddRange(binanceCandles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Save binance candlestick to the DB
        if (binanceCandlesticksDto.Count > 0)
        {
            // Sauvegarder les candles recu de binance dans notre DB
            // Créer un dictionnaire pour le contexte de mappage
            //var context = new Dictionary<string, object> { { "Interval", dbInterval } };
            //context.Add("Symbol", dbSymbol);
            var candlestickToSave = _mapper.Map<IEnumerable<Candlestick>>(binanceCandlesticksDto,
                                                                          opts =>
                                                                          {
                                                                              opts.Items["Interval"] = dbInterval;
                                                                              opts.Items["Symbol"] = dbSymbol;
                                                                          });

            await _candlestickRepository.addCandlesticks(candlestickToSave);
        }
        #endregion

        var candleSticks = new List<CandlestickDto>();
        candleSticks.AddRange(dbCandlesticksDto);
        candleSticks.AddRange(binanceCandlesticksDto);
        candleSticks = candleSticks.OrderBy(c => c.OpenTime).ToList();

        #region Check si toute les candles se suivent bien
        try
        {
            CandlestickHelper.isContinuous(candleSticks);
        }
        catch (Exception error)
        {
            return BadRequest(error.Message);
        }
        #endregion

        HttpContext.Response.Headers.Append("NbCandle", candleSticks.Count + "");

        if(candleSticks.Count > 2000)
        {
            HttpContext.Response.Headers.Append("NbCandleReturned", 2000+"");

            return Ok(candleSticks.Take(2000));
        }

        return Ok(candleSticks);
    }

    public async Task<IEnumerable<CandlestickDto>> getBinanceCandlesticks(string symbol, KlineInterval interval, Gap gap)
    {
        // Get toutes les candles devant être récupéré de binance
        var nbTotalCandlesRequested = gap.getNbTotalCandles(interval);

        // Le nb remaining candles sera utile pour check s'il y a toujours des candles devant être demandé a binance
        var nbRemainingCandles = nbTotalCandlesRequested;

        var startTimeCalculated = new DateTime(gap.FromTime.Ticks);

        var binanceCandlesticks = new List<CandlestickDto>();

        _logger.LogInformation("{nbTotalCandlesRequested} candles to request to binance with interval {interval} for {symbol} From {currentFromTime} to {currentToTime}", nbTotalCandlesRequested, interval, symbol, gap.FromTime, gap.ToTime);

        while (nbRemainingCandles > 0)
        {

            var limit = calculLimitCandle(ref nbRemainingCandles);

            var currentFromTime = new DateTime(startTimeCalculated.Ticks);
            var currentToTime = currentFromTime.AddSeconds(((double)interval * limit) - 1);

            _logger.LogInformation("{limit} Candles requested to binance From {currentFromTime} To {currentToTime}", limit, currentFromTime, currentToTime);

            WebCallResult<IEnumerable<IBinanceKline>> response = await _binanceRestClient.SpotApi.ExchangeData.GetKlinesAsync(symbol, interval, currentFromTime, currentToTime, limit);

            var weigth1m = response.ResponseHeaders?.First(r => r.Key == "x-mbx-used-weight-1m").Value;
            if (weigth1m == null) throw new Exception("The weigth1m is null ??");
            var weigth1mJoin = string.Join("",weigth1m);

            bool success = int.TryParse(weigth1mJoin, out int result);
            if(result >= 5000)
            {
                Thread.Sleep(60000);
            }



            _logger.LogInformation("Le poids est de {poids}", weigth1m);

            // TO DO : Check sur la response le status de binance et retourner en cas d'erreur
            IEnumerable<IBinanceKline> currentCandlesticks = response.Data;


            _logger.LogInformation("{countBinanceCandles} Candles received from binance From {from} To {to}", currentCandlesticks.Count(), currentCandlesticks.First().OpenTime, currentCandlesticks.Last().CloseTime);


            var binanceCandlesticksDto = _mapper.Map<IEnumerable<CandlestickDto>>(currentCandlesticks).OrderBy(c => c.OpenTime).ToList();

            // Il y a t'il des GAP dans la requete faite vers binance ???
            // Si oui faire un log critical car ca ne doit pas arriver et mock les données...
            var Binancegaps = Gap.analyseGap(binanceCandlesticksDto, currentFromTime, currentToTime);

            if (Binancegaps.Count() > 0)
            {
                foreach (var binanceGap in Binancegaps)
                {
                    _logger.LogCritical("There is a gap in binance candles from {fromTime} to {toTime}", binanceGap.FromTime, binanceGap.ToTime);
                    var candlesFixed = binanceGap.completeGap(interval, _logger);
                    binanceCandlesticksDto.AddRange(candlesFixed);
                }
                // Corriger les GAP de binance
                binanceCandlesticksDto = binanceCandlesticksDto.OrderBy((c => c.OpenTime)).ToList();
            }




            startTimeCalculated = currentFromTime.AddSeconds((double)interval * limit);

            // Check si la derniere candles de la response + 1 secondes est bien = au nouveau start time
            var currentLastCloseTime = currentCandlesticks.Last().CloseTime.AddMilliseconds(1);
            if (!(currentLastCloseTime == startTimeCalculated))
            {
                throw new Exception($"The Request from {currentFromTime} to {currentToTime} for the interval {interval} fall in error.\n" +
                                  $"The last candle + 1 millisecond {currentLastCloseTime} is not equal to the newly calculated startTime {startTimeCalculated}");
            }

            try
            {
                CandlestickHelper.isContinuous(binanceCandlesticksDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            binanceCandlesticks.AddRange(binanceCandlesticksDto);
        }

        return binanceCandlesticks;
    }

    public int calculLimitCandle(ref double nbCandlesRequested, int maxLimit = 1000)
    {
        var limit = nbCandlesRequested / maxLimit >= 1 ? maxLimit : nbCandlesRequested;
        nbCandlesRequested = nbCandlesRequested - 1000;
        return (int)limit;
    }

    [Route("CorrectStartTime")]
    [HttpGet]
    public ActionResult<DateTime> getCorrectStartTime([FromQuery] DateTime startTime,
                                       [FromQuery, ModelBinder(BinderType = typeof(KlineIntervalBinder))] KlineInterval interval)
    {
        var intervalHelper = new IntervalHelper(interval);
        intervalHelper.isValidStartTime(startTime, out DateTime correctedStartTime);
        return Ok(correctedStartTime);
    }

    [Route("CorrectEndTime")]
    [HttpGet]
    public ActionResult<DateTime> getCorrectEndTime([FromQuery] DateTime endTime,
                                   [FromQuery, ModelBinder(BinderType = typeof(KlineIntervalBinder))] KlineInterval interval)
    {
        var intervalHelper = new IntervalHelper(interval); 
        intervalHelper.isValidEndTime(endTime, out DateTime correctedEndTime);
        return Ok(correctedEndTime);
    }


}
