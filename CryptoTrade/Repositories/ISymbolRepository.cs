using CryptoTrade.Entities;

namespace CryptoTrade.Repositories;

public interface ISymbolRepository
{
    Task<Symbol?> GetSymbolByName(string symbol);
    Task SaveSymbol(Symbol symbol);
}
