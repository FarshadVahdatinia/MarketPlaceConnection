using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Core.Entities
{
    [Table("MarketPlaceVendor")]
    public class MarketPlaceVendor
    {
        public MarketPlaceVendor()
        {
            MarketPlaceType = new MarketPlaceType(); 
        }
        public int Id { get; set; }
        public int MarketPlaceTypeId { get; set; }
        public int CompanyId { get; set; }
        public string? VendorId { get; set; }
        public string? VendorCode { get; set; }
        public string? VendorSecret { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        public string? Comment { get; set; }

        public MarketPlaceType MarketPlaceType { get; set; }
    }
}
