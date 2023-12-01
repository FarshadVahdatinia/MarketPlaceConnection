using GPOS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Core.Contract
{
    public interface IMarketPlaceRepository
    {
       Task<List<MarketPlaceOrder>> GetAllActiveMarketPlaceOrders(int marketPlaceTypeId);
        Task<List<MarketPlaceType>> GetActiveMarketPlaceTypes();
    }
}
