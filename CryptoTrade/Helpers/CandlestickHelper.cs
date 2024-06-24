using Shared.Models;

namespace CryptoTrade.Helpers;

public class CandlestickHelper
{

    // A continuous Candlesticks is when there are no gap between them
    // So the close time + 1 milliseconds should be equal to the open time of the next candle
    public static Boolean isContinuous(IEnumerable<CandlestickDto> candlesticks)
    {
        CandlestickDto? lastCandle = null;
        foreach (var candlestick in candlesticks)
        {
            if(lastCandle == null)
            {
                lastCandle = candlestick;
                continue;
            }
            if(!(lastCandle.CloseTime.AddMilliseconds(1) == candlestick.OpenTime))
            {
                throw new Exception($"The candle {lastCandle.CloseTime} is not just before the next one {candlestick.OpenTime}");
            }
            lastCandle = candlestick;
        }

        return true;
    }

}
