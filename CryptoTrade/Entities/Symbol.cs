using System.ComponentModel.DataAnnotations;

namespace CryptoTrade.Entities;

public class Symbol
{

    [Key]
    public string Name { get; set; }

    [Required]
    public string BaseAsset { get; set; }

    [Required]
    public string QuoteAsset { get; set; }

    public Symbol(string name, string baseAsset, string quoteAsset)
    {
        Name = name;
        BaseAsset = baseAsset;
        QuoteAsset = quoteAsset;
    }
}
