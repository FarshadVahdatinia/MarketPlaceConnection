using GPOS.Core.Entities;
using GW.Repository.Entities.BaseInformation;
using Microsoft.EntityFrameworkCore;

namespace GPOS.Infrastructure.Context
{
    public class GPOSDbContext : DbContext
    {
        public GPOSDbContext(DbContextOptions<GPOSDbContext> options) : base(options)
        {
          
        }
        public virtual DbSet<Good> Goods { get; set; }
        public virtual DbSet<SetupConfig> SetupConfigs { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<GoodGroup> GoodGroups { get; set; }
        public virtual DbSet<GoodBarcode> GoodBarcodes { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Cash> Cashes { get; set; }
        public virtual DbSet<SellPriceType> SellPriceTypes { get; set; }
        public virtual DbSet<MarketPlaceType> MarketPlaceTypes { get; set; }
        public virtual DbSet<MarketPlaceVendor> MarketPlaceVendors { get; set; }
        public virtual DbSet<MarketPlaceOrder> MarketPlaceOrders { get; set; }
        public virtual DbSet<MarketPlaceOrderDetail> MarketPlaceOrderDetails { get; set; }
        public virtual DbSet<SellPrice> SellPrices { get; set; }
        public virtual DbSet<MarketPlaceRejectReason> MarketPlaceRejectReasons { get; set; }
    }
}
