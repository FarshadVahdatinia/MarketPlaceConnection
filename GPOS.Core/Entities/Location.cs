using GW.Repository.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOS.Core.Entities
{


    public class Location
    {

        public Location()
        {
            //HistFactorDocs = new HashSet<HistFactorDoc>();
            //RetFactors = new HashSet<RetFactor>();
            Cashes = new HashSet<Cash>();
        }

        public int Id { get; set; }


        public string Code { get; set; }

        public string Name { get; set; }

        public bool? CenterBranchType { get; set; }

        public int LocationTypeId { get; set; }

        public string Address { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(10)]
        public string PreTelephoneCode { get; set; }

        [StringLength(20)]
        public string Telephone { get; set; }

        [StringLength(128)]
        public string TelephoneList { get; set; }

        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(255)]
        public string URL { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public int CityId { get; set; }

        public int? AccountId { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }

        public int CompanyId { get; set; }

        public bool? AutoRequestStat { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] VersionCtrl { get; set; }


        //public virtual ICollection<HistFactorDoc> HistFactorDocs { get; set; }


        //public virtual ICollection<RetFactor> RetFactors { get; set; }

        public virtual Company Company { get; set; }

        //public virtual ICollection<Shipment> Shipments { get; set; }
        public ICollection<Cash> Cashes { get; set; }
        public ICollection<SellPrice> SellPrices { get; set; }
    }
}
