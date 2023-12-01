namespace GPOS.Core.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class GoodBarcode
    {
        public GoodBarcode()
        {
            Good = new Good();
        }
        public int Id { get; set; }

        public int GoodId { get; set; }

        [Required]
        [StringLength(255)]
        public string? Barcode { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[]? VersionCtrl { get; set; }

        public virtual Good Good { get; set; }
    }
}
