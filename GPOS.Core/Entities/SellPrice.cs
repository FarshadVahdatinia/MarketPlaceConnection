using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOS.Core.Entities
{

    public class SellPrice
    {
        public SellPrice()
        {
            Good = new Good();
        }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int? ServiceId { get; set; }
        public int GoodId { get; set; }
        public int SellPriceTypeId { get; set; }
        public double Amount { get; set; }
        public int UserId { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public int? SellChangeSourceId { get; set; }
        public Good Good { get; set; }
        public Location Location { get; set; }
        public SellPriceType SellPriceType { get; set; }
        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] VersionCtrl { get; set; }
    }
}
