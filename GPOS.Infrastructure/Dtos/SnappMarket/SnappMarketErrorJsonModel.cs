using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Infrastructure.Dtos.SnappMarket
{
    public class SnappMarketErrorJsonModel
    {
        public SnappMarketAckPickErrorDto? error { get; set; }
        public string? AllErrorsInString { get; set; }
    }
}
