using CrytoTadeUi.Services;
using CrytoTadeUi.Services.CryptoTadeApi;
using CrytoTadeUi.Views;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Syncfusion.Licensing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


// How to add this project to github repository ?
// 1. Create a new repository on GitHub. To avoid errors, do not initialize the new repository with README, license, or gitignore files.
// 2. Open Git Bash.
// 3. Change the current working directory to your local project.
// 4. Initialize the local directory as a Git repository.
// git init

// 5. Add the files in your new local repository. This stages them for the first commit.
// This will include all the files in the directory even the dot net core files.
// How to git ignore the dot net core files ?


// 6. Commit the files that you've staged in your local repository.
// 7. Copy the https url of your newly created repository.
// 8. In the Command prompt, add the URL for the remote repository where your local repository will be pushed.
// 9. Push the changes in your local repository to GitHub.
// 10. Verify the changes on GitHub.

// how to remove after a git add . ?
// git reset


namespace CrytoTadeUi.ViewModels;

public class LoadCandlesViewModel: INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;

    public SelectableTimeIntervalViewModel TimeIntervalVM { get; set; }

    private readonly ExchangeInfoService _exchangeInfoService;
    private readonly ApiService _apiService;
    private ILogger<LoadCandles> _logger;

    public ObservableCollection<String> UniqueQuotes { get;private set;} = new ObservableCollection<String>();
    public ObservableCollection<BinanceSymbolDto> Assets { get; private set; } = new ObservableCollection<BinanceSymbolDto>();

    public String SelectedUniqueQuote { get; set; } = String.Empty;

    public BinanceSymbolDto ? SelectedAsset { get; set; } = null;

    public DateTime SelectedFromDateTime { get; set; } = DateTime.Now;

    public DateTime SelectedToDateTime { get; set; } = DateTime.Now;


    public LoadCandlesViewModel(ExchangeInfoService exchangeInfoService, ILogger<LoadCandles> logger,ApiService apiService)
    {
        _exchangeInfoService = exchangeInfoService;
        _logger = logger;
        _apiService = apiService;

 
        TimeIntervalVM = new SelectableTimeIntervalViewModel();
        TimeIntervalVM.PropertyChanged += TimeIntervalVM_PropertyChanged;
        TimeIntervalVM.SelectedIntervals.CollectionChanged += SelectedIntervals_CollectionChanged;

    }

    private void TimeIntervalVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectableTimeIntervalViewModel.SelectedInterval))
        {
            Console.WriteLine($"Selected Interval: {TimeIntervalVM.SelectedInterval}");
            _logger.LogInformation("Selected Interval: {timeInterval}", string.Join(", ", TimeIntervalVM.SelectedInterval));

        }
    }
    private void SelectedIntervals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Console.WriteLine("Selected Intervals:");
        foreach (var interval in TimeIntervalVM.SelectedIntervals)
        {
            Console.WriteLine(interval);
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task loadUniqueQuotes()
    {
        var uniqueQuotes = await _exchangeInfoService.GetUniqueQuoteAssets();

        foreach (var uniqueQuote in uniqueQuotes)
        {
            UniqueQuotes.Add(uniqueQuote);
        }
    }

    public async Task loadAssetByQuote()
    {
        var assets = await _exchangeInfoService.GetSpotSymbolInformation(SelectedUniqueQuote);

        foreach (var asset in assets)
        {
            Assets.Add(asset);
        }
    }

    public async Task LoadCandlesClicked()
    {
        _logger.LogInformation("Load {assetName} From {fromDateTime} To {toDateTime}", SelectedAsset?.Name, SelectedFromDateTime, SelectedToDateTime);

        // Log selected intervals
        _logger.LogInformation("Selected Interval: {timeInterval}", string.Join(", ", TimeIntervalVM.SelectedIntervals));

        foreach(var interval in TimeIntervalVM.SelectedIntervals)
        {
            DateTime toDateTime = new DateTime(SelectedToDateTime.Year, SelectedToDateTime.Month, SelectedToDateTime.Day,
                                                       SelectedToDateTime.Hour, SelectedToDateTime.Minute,59).AddMilliseconds(999); ;

            try
            {
                var candles = await _apiService.GetCandlesticksAsync(SelectedAsset.Name, interval, SelectedFromDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff"), toDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                foreach (var candle in candles)
                {
                    _logger.LogInformation("Candle: {candle}", candle);
                }

            }catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }


    public void UpdateFromDate(DateTime date)
    {
        SelectedFromDateTime = new DateTime(date.Year, date.Month, date.Day,
                                        SelectedFromDateTime.Hour, SelectedFromDateTime.Minute, SelectedFromDateTime.Second);
    }
    public void UpdateToDate(DateTime date)
    {
        SelectedToDateTime = new DateTime(date.Year, date.Month, date.Day,
                                        SelectedToDateTime.Hour, SelectedToDateTime.Minute, SelectedToDateTime.Second);
    }

    public void UpdateFromTime(TimeSpan time)
    {
        SelectedFromDateTime = new DateTime(SelectedFromDateTime.Year, SelectedFromDateTime.Month, SelectedFromDateTime.Day,
                                        time.Hours, time.Minutes, time.Seconds);
    }
    public void UpdateToTime(TimeSpan time)
    {
        SelectedToDateTime = new DateTime(SelectedToDateTime.Year, SelectedToDateTime.Month, SelectedToDateTime.Day,
                                        time.Hours, time.Minutes, time.Seconds);
    }

}
