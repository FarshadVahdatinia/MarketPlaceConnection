using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Core.Entities
{
    [Table("MarketPlaceType")]
    public class MarketPlaceType
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public string? Comment { get; set; }
        public string? TokenAddress { get; set; }
        public string? Address { get; set; }
        public string? AckAPI { get; set; }
        public string? PickAPI { get; set; }
        public string? AcceptAPI { get; set; }
        public string? RejectAPI { get; set; }
        public string? UsersAddress { get; set; }
        public string? ChangePriceQtyAPI { get; set; }
        public string? DisableItemAPI { get; set; }
        public string? ResponseSuccessCode { get; set; }
        public int? EmployeeId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public int? CustomerId { get; set; }
        public int? CreditCardTypeId { get; set; }
        public DateTime? TokenExpire { get; set; }

        public virtual ICollection<MarketPlaceVendor>? MarketPlaceVendors { get; set; }
        public virtual ICollection<MarketPlaceRejectReason>? MarketPlaceRejectReasons { get; set; }
    }
}
