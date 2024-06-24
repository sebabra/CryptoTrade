using System.ComponentModel.DataAnnotations;

namespace CryptoTrade.Entities;

public class Interval
{
    [Key]
    public int IntervalId { get; set; }
    public string Name {  get; set; }

    public Interval(int intervalId, string name)
    {
        this.IntervalId = intervalId;
        this.Name = name;
    }
}
