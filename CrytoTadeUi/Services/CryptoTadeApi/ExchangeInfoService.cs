using System.Net.Http.Json;
using Shared.Models;

namespace CrytoTadeUi.Services.CryptoTadeApi;

public class ExchangeInfoService
{
    private readonly HttpClient _httpClient;
    private readonly string GET_UNIQUE_QUOTE_ASSETS_URL = "ExchangeInfo/UniqueQuoteAssets";
    private readonly string GET_SPOT_SYMBOL_INFORMATION_URL = "ExchangeInfo";


    public ExchangeInfoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<String>> GetUniqueQuoteAssets()
    {
        var quoteAssets = await _httpClient.GetFromJsonAsync<List<String>>(_httpClient.BaseAddress + GET_UNIQUE_QUOTE_ASSETS_URL);
        return quoteAssets ?? new List<String>();
    }

    public async Task<List<BinanceSymbolDto>> GetSpotSymbolInformation(String quoteAsset)
    {
        var url = $"{_httpClient.BaseAddress}{GET_SPOT_SYMBOL_INFORMATION_URL}?quoteAsset={quoteAsset}";

        var assetsInfo = await _httpClient.GetFromJsonAsync<List<BinanceSymbolDto>>(url);
        return assetsInfo ?? new List<BinanceSymbolDto>();
    }

}
