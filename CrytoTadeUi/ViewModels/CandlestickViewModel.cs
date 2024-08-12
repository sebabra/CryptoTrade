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
    }


}