using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Infrastructure.Dtos.SnappMarket
{
    public class SnappMarketErrorModel
    {
        public string? status { get; set; }
        public SnappMarketAckPickErrorDto? Error { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
        public List<FailedItemsReasons>? failedItems { get; set; }
        public SnappMarketDataResultDto? data { get; set; }
    }
}
