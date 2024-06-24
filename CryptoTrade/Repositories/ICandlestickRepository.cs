using CryptoTrade.Entities;

namespace CryptoTrade.Repositories;

public interface ICandlestickRepository
{
    Task<IEnumerable<Candlestick>> GetCandlesticks(DateTime startTime, DateTime endTime, int interval, string symbol);

    Task addCandlesticks(IEnumerable<Candlestick> candlesticks);
}
