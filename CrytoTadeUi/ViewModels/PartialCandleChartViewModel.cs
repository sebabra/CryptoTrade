using CrytoTadeUi.Models;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrytoTadeUi.ViewModels;

public class PartialCandleChartViewModel
{

    public ObservableCollection<CandlestickData> StockData { get; set; }

    public PartialCandleChartViewModel()
    {
        StockData = new ObservableCollection<CandlestickData> { 
            new CandlestickData (  new DateTime(2024,05,01,0,0,0), 50, 60, 40, 50 ),
            new CandlestickData (  new DateTime(2024,05,02,0,0,0), 50, 60, 40, 50 ),
            new CandlestickData (  new DateTime(2024,05,03,0,0,0), 50, 60, 40, 50 ),
            new CandlestickData (  new DateTime(2024,05,04,0,0,0),50, 60, 40, 50 ),
            new CandlestickData (  new DateTime(2024,05,05,0,0,0), 50, 60, 40, 50 ),
            new CandlestickData (  new DateTime(2024,05,06,0,0,0),50, 60, 40, 50 ),
            new CandlestickData(  new DateTime(2024,05,07,0,0,0), 50, 60, 40, 50 ),
            new CandlestickData(  new DateTime(2024,05,08,0,0,0),50, 60, 40, 50),
            new CandlestickData(  new DateTime(2024,05,09,0,0,0), 50, 60, 40, 50),
            new CandlestickData (  new DateTime(2024,05,10,0,0,0), 50, 60, 40, 50 ),
            new CandlestickData (  new DateTime(2024,05,11,0,0,0), 50, 60, 40, 50),
            new CandlestickData (  new DateTime(2024,05,12,0,0,0), 50, 60, 40, 50),

        };
    }


}
