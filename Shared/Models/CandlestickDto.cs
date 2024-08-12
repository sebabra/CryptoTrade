using System;

namespace Shared.Models
{
    public class CandlestickDto
    {
        public DateTime OpenTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Volume { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal QuoteVolume { get; set; }
        public int TradeCount { get; set; }
        public decimal TakerBuyBaseVolume { get; set; }
        public decimal TakerBuyQuoteVolume { get; set; }
        public bool IsGenerated { get; set; } = false;


        public IchimokuDto Ichimoku { get; set; }

        public CandlestickDto Clone()
        {
            return new CandlestickDto
            {
                OpenTime = OpenTime,
                OpenPrice = OpenPrice,
                HighPrice = HighPrice,
                LowPrice = LowPrice,
                ClosePrice = ClosePrice,
                Volume = Volume,
                CloseTime = CloseTime,
                QuoteVolume = QuoteVolume,
                TradeCount = TradeCount,
                TakerBuyBaseVolume = TakerBuyBaseVolume,
                TakerBuyQuoteVolume = TakerBuyQuoteVolume,
                IsGenerated = IsGenerated
            };
        }
    }

}
