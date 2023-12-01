using GPOS.Core.Contract;
using GPOS.Infrastructure.Context;

namespace GPOS.Infrastructure.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        GPOSDbContext GPOSDbContext { get; }
        ISnappMarketRepository SnappMarketRepository { get; }
        IMarketPlaceRepository MarketPlaceRepository { get; }


        void Commit();
        Task CommitAsync();
    }
}
