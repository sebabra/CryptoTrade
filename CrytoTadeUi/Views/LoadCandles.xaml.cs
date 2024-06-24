using CrytoTadeUi.Services.CryptoTadeApi;
using CrytoTadeUi.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using System.ComponentModel;
namespace CrytoTadeUi.Views;

public partial class LoadCandles : ContentPage
{
	private readonly ExchangeInfoService _exchangeInfoService;
    private ILogger<LoadCandles> _logger;


	public LoadCandles(ExchangeInfoService exchangeInfoService, ILogger<LoadCandles> logger, LoadCandlesViewModel loadCandlesViewModel)
	{
		InitializeComponent();
        _exchangeInfoService = exchangeInfoService;
        _logger = logger;

        BindingContext = loadCandlesViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        // Get the unique quote and add them the the pickerQuote
        displayActivityIndic();
        await ((LoadCandlesViewModel)BindingContext).loadUniqueQuotes();
        hideActivityIndic();

        pickerQuote.IsVisible = true;
    }


    // When the quote is selected from the quote picker
    // We have to get back the corresponding symbol from our API
    // And add them to the asset picker
    private async void OnPickerQuoteSelected(object sender, EventArgs e)
    {

        var picker = (Picker)sender;
        var quoteSelected = (string)picker.SelectedItem;
        // _logger.LogInformation("The quote selected is {quoteSelected}", quoteSelected);
        displayActivityIndic();
        pickerAsset.IsVisible = false;
        await ((LoadCandlesViewModel)BindingContext).loadAssetByQuote();
        hideActivityIndic();
        pickerAsset.IsVisible = true;
        loadCandlesButton.IsEnabled = true;
    }

    private void displayActivityIndic()
    {
        activityIndic.IsVisible = true;
        activityIndic.IsRunning = true;
    }
    private void hideActivityIndic()
    {
        activityIndic.IsVisible = false;
        activityIndic.IsRunning = false;
    }

    public void loadCandlesClicked(object sender, EventArgs e)
    {
        ((LoadCandlesViewModel)BindingContext).LoadCandlesClicked();
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        if(sender == FromDatePicker)
        {
            ((LoadCandlesViewModel)this.BindingContext).UpdateFromDate(e.NewDate);
        }
        else
        {
            ((LoadCandlesViewModel)this.BindingContext).UpdateToDate(e.NewDate);
        }
    }

    private void OnTimeChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TimePicker.Time))
        {

            if(sender == FromTimePicker)
            {
                ((LoadCandlesViewModel)this.BindingContext).UpdateFromTime(((TimePicker)sender).Time);
            }
            else
            {
                ((LoadCandlesViewModel)this.BindingContext).UpdateToTime(((TimePicker)sender).Time);
            }
        }
    }


}