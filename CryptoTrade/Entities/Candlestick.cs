using Binance.Net.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CryptoTrade.Entities;

public class Candlestick
{
    [Key]
    public DateTime OpenTime { get; set; }
    [Required]
    public decimal OpenPrice { get; set; }
    [Required]
    public decimal HighPrice { get; set; }
    [Required]
    public decimal LowPrice { get; set; }
    [Required]
    public decimal ClosePrice { get; set; }
    [Required]
    public decimal Volume { get; set; }
    [Required]
    public DateTime CloseTime { get; set; }
    [Required]
    public decimal QuoteVolume { get; set; }
    [Required]
    public int TradeCount { get; set; }
    [Required]
    public decimal TakerBuyBaseVolume { get; set; }
    [Required]
    public decimal TakerBuyQuoteVolume { get; set; }
    [Required]
    public Interval Interval { get; set; }
    [Required]
    public int IntervalId { get; set; }
    [Required]
    public Symbol Symbol { get; set; }
    [Required]
    public string SymbolName { get; set; }
    [Required]
    public bool IsGenerated { get; set; } = false;
}
