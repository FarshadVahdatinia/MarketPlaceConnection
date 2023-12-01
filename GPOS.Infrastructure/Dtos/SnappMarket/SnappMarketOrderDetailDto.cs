using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Infrastructure.Dtos.SnappMarket
{

    public class SnappMarketOrderDetailDto
    {
        public int StoreId { get; set; }
        public long MarketPlaceOrderId { get; set; }

        public long? ItemId { get; set; }

        public string? Barcode { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Price { get; set; }

        public decimal? PriceVat { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal? VatPercent { get; set; }

        public string? Title { get; set; }
        public long? ParentId { get; set; }

        public int? RejectReasonId { get; set; }
        public int? RejectBy { get; set; }
        public string? ItemsReplace { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountMPShare { get; set; }
    }
}
