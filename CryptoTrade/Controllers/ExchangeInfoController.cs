using AutoMapper;
using Binance.Net.Objects.Models.Spot;
using CryptoTrade.Service.Binance;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace CryptoTrade.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExchangeInfoController : ControllerBase
{

    private readonly IMapper _mapper;
    private readonly ExchangeData _exchangeData;

    public ExchangeInfoController(IMapper mapper, ExchangeData exchangeData)
    {
        _mapper = mapper;
        _exchangeData = exchangeData;
    }

    [Route("UniqueQuoteAssets")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetUniqueQuoteAssets()
    {
        IEnumerable<BinanceSymbol> symbols = await _exchangeData.getFilteredSymbolsInformation();

         symbols = symbols.DistinctBy(s => s.QuoteAsset);

        return Ok(symbols.Select(s=>s.QuoteAsset)
                      .OrderBy(s=>s));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BinanceSymbol>>> GetSpotSymbolInformation([FromQuery] string? quoteAsset)
    {
        IEnumerable<BinanceSymbol> symbols = await _exchangeData.getFilteredSymbolsInformation();

        if (!string.IsNullOrWhiteSpace(quoteAsset))
        {
            symbols = symbols.Where(s => s.QuoteAsset == quoteAsset);
        }

        return Ok(_mapper.Map<IEnumerable<BinanceSymbolDto>>(symbols));
    }
}
