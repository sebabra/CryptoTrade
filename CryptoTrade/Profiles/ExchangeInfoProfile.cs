using AutoMapper;
using Binance.Net.Objects.Models.Spot;
using Shared.Models;

namespace CryptoTrade.Profiles;

public class ExchangeInfoProfile : Profile
{
    public ExchangeInfoProfile()
    {
        CreateMap<BinanceSymbol,BinanceSymbolDto>();
    }
}
