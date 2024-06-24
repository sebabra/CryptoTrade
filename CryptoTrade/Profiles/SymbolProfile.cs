using AutoMapper;
using Binance.Net.Objects.Models.Spot;

namespace CryptoTrade.Profiles;

public class SymbolProfile: Profile
{

    public SymbolProfile() { 
        CreateMap<BinanceSymbol,Entities.Symbol>();
    }
}
