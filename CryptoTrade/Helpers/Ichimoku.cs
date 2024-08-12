using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTrade.Helpers;

// Ichimoku is a trend-following indicator that shows support and resistance levels, and momentum and trend direction.
// The indicator consists of five lines:
// Tenkan-sen (Conversion Line)     : (9-period high + 9-period low) / 2
// Kijun-sen (Base Line)            : (26-period high + 26-period low) / 2
// Senkou Span A (Leading Span A)   : (Tenkan-sen + Kijun-sen) / 2
// Senkou Span B (Leading Span B)   : (52-period high + 52-period low) / 2
// Chikou Span (Lagging Span)       : Close plotted 26 days in the past
// The default parameters of the Ichimoku Cloud are 9, 26, 52, 26.

public class Ichimoku
{
    public int TenkanSen { get; set; }
    public int KijunSen { get; set; }
    public int SenkouSpanB { get; set; }
    public int ChikouSpan { get; set; }
    public List<CandlestickDto> Candles { get; set; }

    // In the constructor, I want to pass the parameters of the Ichimoku Cloud 
    // and the list of candles to calculate the Ichimoku Cloud.
    public Ichimoku(int tenkanSen, int kijunSen, int senkouSpanB, int chikouSpan, List<CandlestickDto> candles)
    {
        TenkanSen = tenkanSen;
        KijunSen = kijunSen;
        SenkouSpanB = senkouSpanB;
        ChikouSpan = chikouSpan;
        Candles = candles;
    }

    // The method CalculateIchimokuCloud will calculate the Ichimoku Cloud
    public void CalculateIchimokuCloud()
    {
        // First, initialize IchimokuDto for each candle
        foreach (var candle in Candles)
        {
            candle.Ichimoku = new IchimokuDto();
        }

        // Calculate Tenkan-sen and Kijun-sen
        for (int i = 0; i < Candles.Count; i++)
        {
            if (i >= TenkanSen - 1)
            {
                decimal high = Candles.GetRange(i - TenkanSen + 1, TenkanSen).Max(c => c.HighPrice);
                decimal low = Candles.GetRange(i - TenkanSen + 1, TenkanSen).Min(c => c.LowPrice);
                Candles[i].Ichimoku.TenkanSen = (high + low) / 2;
            }

            if (i >= KijunSen - 1)
            {
                decimal high = Candles.GetRange(i - KijunSen + 1, KijunSen).Max(c => c.HighPrice);
                decimal low = Candles.GetRange(i - KijunSen + 1, KijunSen).Min(c => c.LowPrice);
                Candles[i].Ichimoku.KijunSen = (high + low) / 2;
            }
        }

        // Calculate Senkou Span A and B and shift forward
        for (int i = 0; i < Candles.Count; i++)
        {
            if (i >= Math.Max(TenkanSen, KijunSen) - 1)
            {
                var senkouSpanA = (Candles[i].Ichimoku.TenkanSen + Candles[i].Ichimoku.KijunSen) / 2;
                if (i + 25 < Candles.Count)
                {
                    Candles[i + 25].Ichimoku.SenkouSpanA = senkouSpanA;
                }
                else if (i + 25 == Candles.Count - 1)
                {
                    Candles.Last().Ichimoku.SenkouSpanA = senkouSpanA;
                }
            }

            if (i >= SenkouSpanB - 1)
            {
                decimal high = Candles.GetRange(i - SenkouSpanB + 1, SenkouSpanB).Max(c => c.HighPrice);
                decimal low = Candles.GetRange(i - SenkouSpanB + 1, SenkouSpanB).Min(c => c.LowPrice);
                var senkouSpanB = (high + low) / 2;
                if (i + 25 < Candles.Count)
                {
                    Candles[i + 25].Ichimoku.SenkouSpanB = senkouSpanB;
                }
                else if (i + 25 == Candles.Count - 1)
                {
                    Candles.Last().Ichimoku.SenkouSpanB = senkouSpanB;
                }
            }
        }

        // Calculate Chikou Span
        /*for (int i = ChikouSpan; i < Candles.Count; i++)
        {
            Candles[i - ChikouSpan].Ichimoku.ChikouSpan = Candles[i].ClosePrice;
        }*/


                     // Calculate Chikou Span
                     
            for (int i = 0; i < Candles.Count - ChikouSpan-1; i++)
            {
                Candles[i].Ichimoku.ChikouSpan = Candles[i + ChikouSpan-1].ClosePrice;
            }

    }
}
