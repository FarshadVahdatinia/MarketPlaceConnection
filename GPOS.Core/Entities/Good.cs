using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOS.Core.Entities
{
    public class Good
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string? Code { get; set; }

        [Required]
        [StringLength(255)]
        public string? Name { get; set; }

        [StringLength(255)]
        public string? LatinName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public int UnitId { get; set; }

        public int? GoodGroupId { get; set; }

        public int GoodTypeId { get; set; }

        [StringLength(255)]
        public string? Mark { get; set; }

        [Column(TypeName = "image")]
        public byte[]? Image { get; set; }

        [StringLength(20)]
        public string? PartNo { get; set; }

        public bool IsActive { get; set; }

        public int? LocationId { get; set; }

        public bool IsLocal { get; set; }

        [StringLength(50)]
        public string? SubCode { get; set; }

        public bool IsBail { get; set; }

        public double TaxPercent { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[]? VersionCtrl { get; set; }

        public bool VATStat { get; set; }

        public int? VATGroupId { get; set; }

        public int? TollGroupId { get; set; }

        public bool NoEntryInv { get; set; }

        public int PackQty { get; set; }

        public bool IsSeason { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Weight { get; set; }

        public bool InternalStat { get; set; }

        public int? BrandId { get; set; }

        public int? ProviderId { get; set; }

        [StringLength(50)]
        public string? ProviderItemCode { get; set; }

        public int? ModelId { get; set; }

        public int? ColorId { get; set; }

        [StringLength(10)]
        public string? GoodSize { get; set; }

        public bool VATNotReceivable { get; set; }

        public int? ParentId { get; set; }

        public bool EShopStat { get; set; }

        public int? SizeTypeId { get; set; }

        public double? GoodLength { get; set; }

        public double? GoodWidth { get; set; }

        public double? GoodHeight { get; set; }

        public int? PersonUsageTypeId { get; set; }

        public int? PersonContactTypeId { get; set; }

        public int? GoodKindId { get; set; }

        public int? GoodClassId { get; set; }

        public int? GoodCategoryId { get; set; }

        public int? GoodDesignId { get; set; }

        public int? GoodMaterialId { get; set; }

        public int? GoodShapeId { get; set; }

        public int? GoodQualityId { get; set; }

        public int? GoodDropId { get; set; }

        public int? GoodKalitehId { get; set; }

        public int? SeasonId { get; set; }

        public bool RcvSalesLocn { get; set; }

        public int? ExpireDay { get; set; }

        public int? PluNo { get; set; }

        public int? Tare { get; set; }

        public bool JetPrintStat { get; set; }

        [StringLength(255)]
        public string? ShortName { get; set; }

        public int? UserId { get; set; }

        public int? EditorId { get; set; }

        [StringLength(10)]
        public string? CreateDate { get; set; }

        [StringLength(10)]
        public string? EditDate { get; set; }

        public int PurchaseTypeId { get; set; }

        public virtual ICollection<Good> Good1 { get; set; }

        public virtual Good Good2 { get; set; }

        public virtual GoodGroup GoodGroup { get; set; }
        public virtual ICollection<GoodBarcode> GoodBarcodes { get; set; }
        public virtual ICollection<SellPrice> SellPrices { get; set; }
    }
}
