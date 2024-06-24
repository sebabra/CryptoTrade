using System;

namespace Shared.Models
{
    public class BinanceSymbolDto
    {
        public string Name { get; set; } = String.Empty;
        public string BaseAsset { get; set; } = String.Empty;
        public int BaseAssetPrecision { get; set; }
        public string QuoteAsset { get; set; } = String.Empty;
        public int QuoteAssetPrecision { get; set; }
    }
}


