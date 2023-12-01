using GPOS.Core.Contract;
using GPOS.Core.Entities;
using GPOS.Core.Enums;
using GPOS.Infrastructure.Context;
using GPOS.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Infrastructure.Repository
{
    public class MarketPlaceRepository:IMarketPlaceRepository
    {
        private readonly GPOSDbContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly IConfiguration _configuration;

        public MarketPlaceRepository(GPOSDbContext dbContext, ILogger<UnitOfWork> logger, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<MarketPlaceOrder>> GetAllActiveMarketPlaceOrders(int marketPlaceTypeId)
        {
            var query = await _dbContext.MarketPlaceOrders.AsNoTracking().Include(x => x.MarketPlaceOrderDetails).Where(x => x.IsActive && x.MarketPlaceTypeId == marketPlaceTypeId).ToListAsync();
            return query;
        }

        public async Task<List<MarketPlaceType>> GetActiveMarketPlaceTypeIds()
        {
            var query = await _dbContext.MarketPlaceTypes.AsNoTracking().Where(x => x.IsActive).ToListAsync();
            return query;
        }
    }
}
