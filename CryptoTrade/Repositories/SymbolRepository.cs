using CryptoTrade.DbContexts;
using CryptoTrade.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrade.Repositories;

public class SymbolRepository : ISymbolRepository
{
    public readonly CryptoTradeContext _context;

    public SymbolRepository(CryptoTradeContext context)
    {
        _context = context;
    }

    async Task<Symbol?> ISymbolRepository.GetSymbolByName(string name)
    {
        return await _context.Symbols.FirstOrDefaultAsync(s => s.Name == name);
    }

    async Task ISymbolRepository.SaveSymbol(Symbol symbol)
    {
        await _context.Symbols.AddAsync(symbol);
    }
}
