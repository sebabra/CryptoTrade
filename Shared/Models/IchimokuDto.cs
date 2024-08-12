using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class IchimokuDto
    {
        public decimal TenkanSen { get; set; }
        public decimal KijunSen { get; set; }
        public decimal SenkouSpanA { get; set; }

        public decimal SenkouSpanB { get; set; }
        public decimal ChikouSpan { get; set; }

        public IchimokuDto(decimal tenkanSen = 0, decimal kijunSen = 0, decimal senkouSpanA = 0, decimal senkouSpanB = 0, decimal chikouSpan = 0)
        {
            TenkanSen = tenkanSen;
            KijunSen = kijunSen;
            SenkouSpanA = senkouSpanA;
            SenkouSpanB = senkouSpanB;
            ChikouSpan = chikouSpan;
        }
    }
}
