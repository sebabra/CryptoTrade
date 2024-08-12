using CrytoTadeUi.Services.CryptoTadeApi;
using CrytoTadeUi.ViewModels;
using CrytoTadeUi.ViewsPartial;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using System.ComponentModel;
namespace CrytoTadeUi.Views;

public partial class LoadCandles : ContentPage
{
    private readonly ExchangeInfoService _exchangeInfoService;
    private readonly ILogger<LoadCandlesSelection> _logger;
    private readonly LoadCandlesViewModel _loadCandlesViewModel;


    public LoadCandles(ExchangeInfoService exchangeInfoService, ILogger<LoadCandlesSelection> logger, LoadCandlesViewModel loadCandlesViewModel)
    {
        InitializeComponent();
        _exchangeInfoService = exchangeInfoService;
        _logger = logger;
        _loadCandlesViewModel = loadCandlesViewModel;

        // Initialize the partial view
        loadCandlesSelection.Initialize(_exchangeInfoService, _logger, _loadCandlesViewModel);
    }

    protected override void OnAppearing()
	{
        base.OnAppearing();
        loadCandlesSelection.OnAppearing();
    }

}