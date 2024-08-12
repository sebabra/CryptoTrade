using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrytoTadeUi.Models;

public class CandlestickData
{
    public DateTime Date { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }

    public CandlestickData(DateTime date, decimal open, decimal high, decimal low, decimal close)
    {
        Date = date;
        Open = open;
        High = high;
        Low = low;
        Close = close;
    }

}