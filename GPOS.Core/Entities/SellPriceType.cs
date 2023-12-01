using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOS.Core.Entities
{


    public partial class SellPriceType
    {
      
        public SellPriceType()
        {
            //People = new HashSet<Person>();
            //PollSellPriceTypeLimits = new HashSet<PollSellPriceTypeLimit>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[]? VersionCtrl { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Person> People { get; set; }
        //public virtual ICollection<PollSellPriceTypeLimit> PollSellPriceTypeLimits { get; set; }
        public virtual ICollection<SellPrice> SellPrices { get; set; }
    }
}
