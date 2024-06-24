using CryptoTrade.Entities;

namespace CryptoTrade.Repositories;

public interface IIntervalRepository
{
    Task<Interval?> GetInterval(int intervalId);
}
