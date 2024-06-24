using CryptoTrade.DbContexts;
using CryptoTrade.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrade.Repositories;

public class CandlestickRepository : ICandlestickRepository
{

    private readonly CryptoTradeContext _context;

    public CandlestickRepository(CryptoTradeContext context)
    {
        _context = context;
    }

    async Task<IEnumerable<Candlestick>> ICandlestickRepository.GetCandlesticks(DateTime startTime, DateTime endTime, int interval, string symbol)
    {
        var candlesticks = await _context.Candlesticks
                                .Where(c => c.OpenTime >= startTime && c.OpenTime <= endTime &&
                                       c.IntervalId == interval &&
                                       c.SymbolName == symbol)
                                .OrderBy(c => c.OpenTime)
                                .ToListAsync();
        return candlesticks;
    }

    async Task ICandlestickRepository.addCandlesticks(IEnumerable<Candlestick> candlesticks)
    {
        _context.ChangeTracker.AutoDetectChangesEnabled = false;
        await _context.Candlesticks.AddRangeAsync(candlesticks);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.AutoDetectChangesEnabled = true;

    }

}
