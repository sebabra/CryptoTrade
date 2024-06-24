using CryptoTrade.DbContexts;
using CryptoTrade.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrade.Repositories;

public class IntervalRepository : IIntervalRepository
{

    private readonly CryptoTradeContext _context;

    public IntervalRepository(CryptoTradeContext context)
    {
        _context = context;
    }

    async Task<Interval?> IIntervalRepository.GetInterval(int intervalId)
    {
        return await _context.Intervals.FirstOrDefaultAsync(i => i.IntervalId == intervalId);
    }

}
