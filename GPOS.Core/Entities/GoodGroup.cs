namespace GPOS.Core.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    
    public  class GoodGroup
    {
        public GoodGroup()
        {
            Goods = new HashSet<Good>();
            GoodGroup1 = new HashSet<GoodGroup>();
            GoodGroup2 = new GoodGroup();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[]? VersionCtrl { get; set; }

        public virtual ICollection<Good> Goods { get; set; }

        public virtual ICollection<GoodGroup> GoodGroup1 { get; set; }

        public virtual GoodGroup GoodGroup2 { get; set; }
    }
}
