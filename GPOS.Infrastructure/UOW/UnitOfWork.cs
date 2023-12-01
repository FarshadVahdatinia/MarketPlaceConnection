using GPOS.Core.Contract;
using GPOS.Infrastructure.Context;
using GPOS.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GPOSDbContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;
        private ISnappMarketRepository _snappMarketRepository;
        private IMarketPlaceRepository _marketPlaceRepository;
        private readonly IConfiguration _configuration;

        public UnitOfWork(GPOSDbContext gposDbContext, ILogger<UnitOfWork> logger, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = gposDbContext;
            _configuration = configuration;

        }


        public ISnappMarketRepository SnappMarketRepository
        {
            get
            {
                if (_snappMarketRepository == null)
                    _snappMarketRepository = new SnappMarketRepository(_dbContext, _logger, _configuration);

                return _snappMarketRepository;
            }
        }

        public IMarketPlaceRepository MarketPlaceRepository
        {
            get
            {
                if (_marketPlaceRepository == null)
                    _marketPlaceRepository = new MarketPlaceRepository(_dbContext, _logger, _configuration);

                return _marketPlaceRepository;
            }
        }

        public GPOSDbContext GPOSDbContext
        {
            get
            {
                return _dbContext;
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

      
    }
}
