using Binance.Net.Enums;
using CryptoTrade.Extension;
using Shared.Models;

namespace CryptoTrade.Helpers;

public class Gap
{
    // Le from time d'un gap est toujours ce que représente l'open time d'une candle
    public DateTime FromTime { get; }
    // Le to time d'un gap est toujours ce que représente le close time d'une candle
    public DateTime ToTime { get; }

    public double NbTotalSecondsRequested { get; }

    public CandlestickDto? FromCandleGap { get; }
    public CandlestickDto? ToCandleGap { get; }


    public Gap(DateTime fromTime, DateTime toTime, CandlestickDto? fromCandleGap = null, CandlestickDto? toCandleGap = null)
    {
        FromTime = fromTime;
        ToTime = toTime;
        NbTotalSecondsRequested = (ToTime.AddMilliseconds(1) - FromTime).TotalSeconds;
        FromCandleGap = fromCandleGap;
        ToCandleGap = toCandleGap;
    }

    public IEnumerable<CandlestickDto> completeGap(KlineInterval interval,ILogger logger)
    {
        List<CandlestickDto> candles = new List<CandlestickDto>();
        var fromTimeClean = FromTime.getCorrectedCandleOpenTime(interval);
        var toTimeClean = ToTime.getCorrectedCandleCloseTime(interval);

        var nbTotalSeconds = (toTimeClean.AddMilliseconds(1) - fromTimeClean).TotalSeconds;
        var nbTotalCandle = nbTotalSeconds / (double)interval;

        if(!(Math.Floor(nbTotalCandle) == nbTotalCandle))
        {
            throw new Exception("Le calcul n'est pas bon");
        }

        if (FromCandleGap == null) throw new Exception("Candle is null");

        logger.LogCritical("The candle from {from} will changed to {fromCorrected}",FromCandleGap.OpenTime, fromTimeClean);
        logger.LogCritical("The candle to {to} will changed to {toCorrected}", FromCandleGap.CloseTime, fromTimeClean.AddSeconds((int)interval).AddMilliseconds(-1));

        logger.LogCritical("The Gap of {nb} will be fill up with the previous candle from {from} to {to}",nbTotalCandle,fromTimeClean,toTimeClean);


        FromCandleGap.OpenTime = fromTimeClean;
        FromCandleGap.CloseTime = fromTimeClean.AddSeconds((int)interval).AddMilliseconds(-1);
        FromCandleGap.IsGenerated = true;

        nbTotalCandle--;
        while (nbTotalCandle > 0)
        {
            fromTimeClean = fromTimeClean.AddSeconds((int)interval);

            var newCandleCopied = FromCandleGap.Clone();

            newCandleCopied.IsGenerated = true;

            newCandleCopied.OpenTime = fromTimeClean;
            newCandleCopied.CloseTime = fromTimeClean.AddSeconds((int)interval).AddMilliseconds(-1); ;
            nbTotalCandle--;

            candles.Add(newCandleCopied);
        }


        return candles;
    }

    public double getNbTotalCandles(KlineInterval interval)
    {
        return NbTotalSecondsRequested / (double)interval;
    }

    // TO DO : Faire DES unit test sur cette méthod
    public static IEnumerable<Gap> analyseGap(IEnumerable<CandlestickDto> candlesticks,
                                              DateTime startTime,
                                              DateTime endTime)
    {
        List<Gap> gaps = new List<Gap>();

        // Si pas de candlesticks alors un gap du début a la fin
        // On retourne ce gap
        if (candlesticks == null || candlesticks.Count() == 0)
        {
            gaps.Add(new Gap(startTime, endTime));
            return gaps;
        }

        var firstCandlestick = candlesticks.First();
        var lastCandlestick = candlesticks.Last();

        // SI le startTime est avant l'open time de la 1er candlestick
        // Alors il y a un gap du startTime jusqu'au openTime de la 1er candlestick
        if (startTime < firstCandlestick.OpenTime)
        {
            gaps.Add(new Gap(startTime, firstCandlestick.OpenTime.AddMilliseconds(-1)));
        }

        // Loop sur les candlesticks pour détecter d'éventuel gap
        CandlestickDto? previousCandle = null;
        foreach (var currentCandle in candlesticks)
        {
            if (previousCandle == null)
            {
                previousCandle = currentCandle;
                continue;
            }

            // Check si la previous candle close time +1 milli secondes est bien égale a la current candle open time
            // Si ce n'est pas le cas alors gap entre
            // la previous candle close time +1 et current candle open time - 1
            if (!(previousCandle.CloseTime.AddMilliseconds(1) == currentCandle.OpenTime))
            {
                gaps.Add(new Gap(previousCandle.CloseTime.AddMilliseconds(1), currentCandle.OpenTime.AddMilliseconds(-1),previousCandle,currentCandle));
            }
            previousCandle = currentCandle;
        }

        // Si le endTime est aprés le close time de la derniere candlestick
        // Alors il y a un gap de la derniere candlestick + 1 jusqu'au endtime
        if (endTime > lastCandlestick.CloseTime)
        {
            gaps.Add(new Gap(lastCandlestick.CloseTime.AddMilliseconds(1), endTime));
        }

        return gaps;
    }

}
