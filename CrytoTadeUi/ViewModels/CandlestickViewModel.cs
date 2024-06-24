using CrytoTadeUi.Models;
using CrytoTadeUi.Services;
using Shared.Models;
using System.Collections.ObjectModel;

namespace CrytoTadeUi.ViewModels;

public class CandlestickViewModel
{
    public ObservableCollection<CandlestickData> Candlesticks { get; set; }
    private readonly ApiService _apiService;


    public CandlestickViewModel()
    {
        _apiService = new ApiService(new HttpClient { BaseAddress = new Uri("https://localhost:7240/api/") });
        Candlesticks = new ObservableCollection<CandlestickData>();
        LoadCandle(); // Appel de la méthode de chargement des données
    }

    // Méthode pour charger les données
    public async Task LoadCandle()
    {
        //var candlesticksFromApi = await _apiService.GetCandlesticksAsync();

        // Supposant que GetCandlesticksAsync retourne une liste de CandlestickData
        /*foreach (var candleDto in candlesticksFromApi)
        {
            Candlesticks.Add(new CandlestickData(candleDto.OpenTime,
                                                 candleDto.OpenPrice,
                                                 candleDto.HighPrice,
                                                 candleDto.LowPrice,
                                                 candleDto.ClosePrice));
        }*/

        // Si GetCandlesticksAsync ne retourne pas directement une liste utilisable,
        // ajustez la logique ci-dessus en conséquence.
    }

}