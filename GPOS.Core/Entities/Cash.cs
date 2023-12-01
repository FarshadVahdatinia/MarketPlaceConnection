using GPOS.Core.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GW.Repository.Entities.BaseInformation
{
    public class Cash 
    {
        public Cash()
        {
            Location = new Location();
        }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int CashNo { get; set; }
        [MaxLength(255)]
        public string? Name { get; set; }
        public int? CashTypeId { get; set; }
        [DefaultValue("1")]
        public int? AccountId { get; set; }
        public bool IsActive { get; set; }
        public int? Token { get; set; }
        [MaxLength(50)]
        public string? SerialNo { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [MaxLength(255)]
        public string? MachineName { get; set; }
        public long? CashIdent { get; set; }
        [DefaultValue("1")]
        public int? CashFinanceTypeId { get; set; }
        public int? DepartmentId { get; set; }
        [DefaultValue("1")]
        public bool? PaymentStat { get; set; }
        [MaxLength(50)]
        [DefaultValue("GposClient")]
        public string DBName { get; set; }
        [DefaultValue("0")]
        public long? MaxCashAmount { get; set; }
        [DefaultValue("0")]
        public long? HintCashAmount { get; set; }
        [DefaultValue("0")]
        public bool? IsOnline { get; set; }
        [DefaultValue("0")]
        public int? IsDelivery { get; set; }
        public byte[]? VersionCtrl { get; set; }
        [ForeignKey(nameof(LocationId))]
        public Location Location { get; set; }
        //public virtual ICollection<PCPOSCash> PCPOSCashes { get; set; }
    }
}
