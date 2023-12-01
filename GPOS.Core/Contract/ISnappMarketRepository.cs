using GPOS.Core.Dtos;
using GPOS.Core.Entities;
using GPOS.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace GPOS.Core.Contract
{
    public interface ISnappMarketRepository
    {
        Task<int> AddAsync(MarketPlaceOrder entity);
        MarketPlaceOrder SnappMarketInitialOrder(IFormCollection dto);
        Task<string?> ChangeSnappMarketStatusApi(int marketPlaceOrderId, MarketPlaceStatusEnum marketPlaceStatus, MarketPlaceRejectDto? rejectDto);
        Task DirectRejectOrder(string orderCode);
        Task ChangeStatusAsync(MarketPlaceStatusEnum phaseApi, int marketPlaceOrderId, string error, int? rejectReasonId = null);
        Task<MyResponse<SnappMarketNullResponseDto>> StockUpdateAsync(SnappMarketInventorySync dto);
        Task<MyResponse<SnappMarketNullResponseDto>> AddOrUpdateProductAsync(SnappMarketAddOrUpdateProductDto dto);
    }
}
