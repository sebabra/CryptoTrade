using AutoMapper;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoTrade.Entities;
using Shared.Models;

namespace CryptoTrade.Profiles;

public class CandlestickProfile : Profile
{
    public CandlestickProfile()
    {
        CreateMap<Entities.Candlestick, CandlestickDto>();
        CreateMap<IBinanceKline, CandlestickDto>();

        CreateMap<CandlestickDto, Entities.Candlestick>().AfterMap((src, dest,context) =>
        {
            var interval = (Interval)context.Items["Interval"];
            var symbol = (Symbol)context.Items["Symbol"];

            dest.Interval = interval;
            dest.Symbol = symbol;
        });
       
    }
}
