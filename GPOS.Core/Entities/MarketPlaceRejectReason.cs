using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOS.Core.Entities
{
    public class MarketPlaceRejectReason 
    {
        public MarketPlaceRejectReason()
        {
            MarketPlaceType = new MarketPlaceType();
        }
        public int Id { get; set; }
        public int? MarketPlaceTypeId { get; set; }
        public int Code { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public string? Comment { get; set; }
        public int? Level { get; set; }
        public int? CancelType { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[]? VersionCtrl { get; set; }
        public MarketPlaceType MarketPlaceType { get; set; }
    }
}
