using AutoMapper;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;

namespace CryptoTrade.Service.Binance;

public class ExchangeData
{
    private readonly BinanceRestClient _binanceRestClient;

    public ExchangeData(BinanceRestClient binanceRestClient)
    {
        _binanceRestClient = binanceRestClient;
    }

    // Get Back only the symbols with
    // status = trading
    // Can be buy direct in market
    // Spot account type
    public async Task<IEnumerable<BinanceSymbol>> getFilteredSymbolsInformation()
    {
        var res = await _binanceRestClient.SpotApi.ExchangeData.GetExchangeInfoAsync();

        IEnumerable<BinanceSymbol> symbols = res.Data.Symbols;

        // Only the Status Trading 
        symbols = symbols.Where(s => s.Status == SymbolStatus.Trading);
        // Can be buy in Market --> The order will be executed at the best price available at that time in the order book.
        symbols = symbols.Where(s => s.OrderTypes.Contains(SpotOrderType.Market));
        /// Spot account type
        //symbols = symbols.Where(s => s.Permissions.Contains(AccountType.Spot));

        return symbols.OrderBy(s=>s.BaseAsset).ThenBy(s=>s.QuoteAsset).ToList();
    }

    public async Task<BinanceSymbol?> getSymbolInformation(string symbol)
    {
        var symbols = await getFilteredSymbolsInformation();
        return symbols.FirstOrDefault(s=>s.Name == symbol);
    }


}


