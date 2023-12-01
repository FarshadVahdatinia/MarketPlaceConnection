using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Core.Dtos
{
    public class SnappMarketInventorySync
    {
        public SnappMarketInventorySync()
        {
            stocks = new List<SnappMarketStockDto>();
        }
        public string? vendorCode { get; set; }
        public List<SnappMarketStockDto> stocks { get; set; }
    }
}
