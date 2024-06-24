using Binance.Net.Enums;
using CryptoTrade.Helpers;

namespace CryptoTrade.Extension;

public static class DateTimeExtension
{


    // Retourne l'open time le plus proche de la dateTime passé pour l'interval passé
    // L'open time le plus proche est toujours <= au dateTime passé
    public static DateTime getCorrectedCandleOpenTime(this DateTime dateTime,KlineInterval interval)
    {
        var intervalHelper = new IntervalHelper(interval);
        intervalHelper.isValidStartTime(dateTime, out DateTime correctedStartTime);
        return correctedStartTime;
    }

    // Retourne le close time le plus proche de la dateTime passé pour l'interval passé
    // Le close time le plus proche est toujours >= au dateTime pasé

    public static DateTime getCorrectedCandleCloseTime(this DateTime dateTime,KlineInterval interval)
    {
        var intervalHelper = new IntervalHelper(interval);
        intervalHelper.isValidEndTime(dateTime, out DateTime correctedEndTime);
        return correctedEndTime;
    }

}
