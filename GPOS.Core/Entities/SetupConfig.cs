using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOS.Core.Entities
{
    [Table("SetupConfig")]
    public class SetupConfig
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Value { get; set; }

        public int LevelChange { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(20)]
        public string DataType { get; set; }

        [StringLength(128)]
        public string FactoryValue { get; set; }

        [StringLength(1000)]
        public string PossibleValues { get; set; }

        public bool? ReadOnly { get; set; }

        public int? CompanyId { get; set; }

        public int? CashId { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] VersionCtrl { get; set; }
    }
}
