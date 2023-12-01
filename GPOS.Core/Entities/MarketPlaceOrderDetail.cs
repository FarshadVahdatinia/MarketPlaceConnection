using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Core.Entities
{
    [Table("MarketPlaceOrderDetail")]
    public class MarketPlaceOrderDetail
    {
        public MarketPlaceOrderDetail()
        {
            MarketPlaceOrder = new MarketPlaceOrder();
        }
        public int Id { get; set; }
        public int MarketPlaceOrderId { get; set; }
        public int? GoodId { get; set; }
        public string? Barcode { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceVat { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? VatPercent { get; set; }
        public string? title { get; set; }
        public int? ParentId { get; set; }
        public int? RejectReasonId { get; set; }
        public int? RejectBy { get; set; }
        public string? GoodsReplace { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountMPShare { get; set; }
        public string? Comment { get; set; }

        [ForeignKey("MarketPlaceOrderId")]
        public MarketPlaceOrder MarketPlaceOrder { get; set; }
    }
}
