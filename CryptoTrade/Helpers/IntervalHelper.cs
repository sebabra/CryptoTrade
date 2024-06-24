using Binance.Net.Enums;
using CryptoTrade.Entities;
using System;

namespace CryptoTrade.Helpers;

public class IntervalHelper
{
    private readonly KlineInterval _interval;

    public IntervalHelper(KlineInterval interval)
    {
        _interval = interval;
    }


    public Boolean isValidEndTime(DateTime endTime)
    {
        return this.isValidStartTime(endTime.AddMilliseconds(1));
    }

    public Boolean isValidEndTime(DateTime endTime, out DateTime correctedEndTime)
    {
        var res = this.isValidStartTime(endTime.AddMilliseconds(1), out correctedEndTime);

        if (res)
        {
            correctedEndTime = correctedEndTime.AddMilliseconds(-1);
            return res;
        }
        else
        {
            correctedEndTime = correctedEndTime.AddSeconds((int)_interval).AddMilliseconds(-1);
            return res;
        }

    }


    public Boolean isValidStartTime(DateTime startTime)
    {
        return isValidStartTime(startTime, out _);
    }


    public Boolean isValidStartTime(DateTime startTime, out DateTime correctedStartTime)
    {

        correctedStartTime = GetClosestValidStartTime(startTime); // Utilisez la méthode définie précédemment pour obtenir la DateTime corrigée

       return correctedStartTime == startTime;
    }

    private DateTime GetClosestValidStartTime(DateTime startTime)
    {
        switch (_interval)
        {
            case KlineInterval.OneSecond:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second);

            case KlineInterval.OneMinute:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, 0);

            case KlineInterval.ThreeMinutes:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute - startTime.Minute % 3, 0);

            case KlineInterval.FiveMinutes:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute - startTime.Minute % 5, 0);

            case KlineInterval.FifteenMinutes:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute - startTime.Minute % 15, 0);

            case KlineInterval.ThirtyMinutes:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute - startTime.Minute % 30, 0);

            case KlineInterval.OneHour:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, 0, 0);

            case KlineInterval.TwoHour:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour - startTime.Hour % 2, 0, 0);

            case KlineInterval.FourHour:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour - startTime.Hour % 4, 0, 0);

            case KlineInterval.SixHour:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour - startTime.Hour % 6, 0, 0);

            case KlineInterval.EightHour:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour - startTime.Hour % 8, 0, 0);

            case KlineInterval.TwelveHour:
                // Ajuste l'heure pour le début du cycle de 12 heures le plus proche vers le bas
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour < 12 ? 0 : 12, 0, 0);

            case KlineInterval.OneDay:
                return new DateTime(startTime.Year, startTime.Month, startTime.Day, 0, 0, 0);

            case KlineInterval.ThreeDay:
                // Calcule le nombre de jours depuis une date de référence, puis retranche le reste de la division par 3
                long totalDays = (long)(startTime - new DateTime(1, 1, 1)).TotalDays;
                long daysSinceLastValid = totalDays % 3;
                return startTime.AddDays(-daysSinceLastValid).Date;

            case KlineInterval.OneWeek:
                // Ajuste au début de la semaine (en supposant que la semaine commence le lundi)
                int daysUntilLastMonday = (int)startTime.DayOfWeek - (int)DayOfWeek.Monday;
                if (daysUntilLastMonday < 0) daysUntilLastMonday += 7; // Pour les cas où le jour actuel est dimanche (-1)
                return startTime.AddDays(-daysUntilLastMonday).Date;

            case KlineInterval.OneMonth:
                return new DateTime(startTime.Year, startTime.Month, 1, 0, 0, 0);

            default:
                throw new NotImplementedException($"La logique pour ajuster à l'intervalle {_interval} n'est pas implémentée.");
        }
    }



}
