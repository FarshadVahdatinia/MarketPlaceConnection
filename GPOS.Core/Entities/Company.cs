namespace GW.Repository.Entities.BaseInformation
{
    using GPOS.Core.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Company
    {
        public Company()
        {
            Locations = new HashSet<Location>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(50)]
        public string MachineName { get; set; }

        [StringLength(50)]
        public string DBName { get; set; }

        public bool EnterpriseCompany { get; set; }

        public bool IsActive { get; set; }

        public bool IsDistributed { get; set; }

        public bool AccStat { get; set; }

        public bool? OrderStat { get; set; }

        public int? SupplyRegionId { get; set; }

        public int? CityId { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public int? CompanyTypeId { get; set; }

        public int? KeyCode { get; set; }
        public byte[]? VersionCtrl { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}
