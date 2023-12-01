using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Infrastructure.Dtos.SnappMarket
{
    public class AcceptOrderDto
    {
        public int packingPrice { get; set; }
        public int delta { get; set; }
        public int deliveryTime { get; set; }
        public int riderPickupTime { get; set; }
    }
}
