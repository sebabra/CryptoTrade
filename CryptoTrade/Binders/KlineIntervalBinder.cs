using Binance.Net.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace CryptoTrade.Binders;

public class KlineIntervalBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        KlineInterval interval;
        try
        {
            if (TryParseKlineInterval(value, out interval))
            {
                bindingContext.Result = ModelBindingResult.Success(interval);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(modelName, $"{value} is not a valid interval format");
            }
        }
        catch (Exception ex)
        {
            bindingContext.ModelState.TryAddModelError(modelName, $"Error parsing interval: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    private bool TryParseKlineInterval(string value, out KlineInterval interval)
    {
        interval = KlineInterval.OneSecond; // Default value, will be overridden
        bool parsed = true;

        switch (value)
        {
            case "1s":
                interval = KlineInterval.OneSecond;
                break;
            case "1m":
                interval = KlineInterval.OneMinute;
                break;
            case "3m":
                interval = KlineInterval.ThreeMinutes;
                break;
            case "5m":
                interval = KlineInterval.FiveMinutes;
                break;
            case "15m":
                interval = KlineInterval.FifteenMinutes;
                break;
            case "30m":
                interval = KlineInterval.ThirtyMinutes;
                break;
            case "1h":
                interval = KlineInterval.OneHour;
                break;
            case "2h":
                interval = KlineInterval.TwoHour;
                break;
            case "4h":
                interval = KlineInterval.FourHour;
                break;
            case "6h":
                interval = KlineInterval.SixHour;
                break;
            case "8h":
                interval = KlineInterval.EightHour;
                break;
            case "12h":
                interval = KlineInterval.TwelveHour;
                break;
            case "1d":
                interval = KlineInterval.OneDay;
                break;
            case "3d":
                interval = KlineInterval.ThreeDay;
                break;
            case "1w":
                interval = KlineInterval.OneWeek;
                break;
            case "1M":
                interval = KlineInterval.OneMonth;
                break;
            default:
                // If the format doesn't match, attempt to parse as an integer (number of seconds)
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int seconds))
                {
                    if (Enum.IsDefined(typeof(KlineInterval), seconds))
                    {
                        interval = (KlineInterval)seconds;
                    }
                    else
                    {
                        // Si les secondes ne correspondent à aucune valeur définie dans l'énumération, considérez cela comme non valide.
                        parsed = false;
                    }
                }
                else
                {
                    parsed = false;
                }
                break;
        }

        return parsed;
    }
}
