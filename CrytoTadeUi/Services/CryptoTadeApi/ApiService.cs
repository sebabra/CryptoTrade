using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace CrytoTadeUi.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string GET_CANDLES_URL = "Candlestick?symbol=BTCUSDT&interval=1m&startTime=2024-02-01T00:00:00.000Z&endTime=2024-02-01T23:59:59.999Z";

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<List<CandlestickDto>> GetCandlesticksAsync(string symbol, string interval, string startTime, string endTime)
    {
        HttpResponseMessage response = null;

        try
        {

            var uri = BuildCandlestickUri(symbol, interval, startTime, endTime);



            // Remplacer 'List<CandlestickData>' par le type de retour approprié
            response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to List<CandlestickDto>
                var candlesticks = await response.Content.ReadFromJsonAsync<List<CandlestickDto>>();
                return candlesticks ?? new List<CandlestickDto>();
            }
            else
            {
                // Read the error message from the response content
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erreur de requête : {errorMessage}");
                throw new HttpRequestException($"Erreur de requête : {response.StatusCode}, Détails : {errorMessage}");
            }
        }
        catch (NotSupportedException e)
        {
            // Le contenu n'est pas au format JSON ou est non lisible
            Console.WriteLine($"Erreur de format de données : {e.Message}");
            throw e;
        }
        catch (JsonException e)
        {
            // La désérialisation JSON a échoué
            Console.WriteLine($"Erreur de désérialisation : {e.Message}");
            throw e;
        }



    }

    private Uri BuildCandlestickUri(string symbol, string interval, string startTime, string endTime)
    {
        var builder = new UriBuilder(_httpClient.BaseAddress + "Candlestick");
        var query = HttpUtility.ParseQueryString(builder.Query);

        query["symbol"] = symbol;
        query["interval"] = interval;
        query["startTime"] = startTime;
        query["endTime"] = endTime;

        builder.Query = query.ToString();
        return builder.Uri;
    }


}
