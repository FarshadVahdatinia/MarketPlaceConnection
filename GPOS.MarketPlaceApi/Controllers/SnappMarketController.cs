using GPOS.Core.Dtos;
using GPOS.Core.Enums;
using GPOS.Infrastructure.UOW;
using Microsoft.AspNetCore.Mvc;

namespace GPOS.MarketPlaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnappMarketController : ControllerBase
    {
        private readonly ILogger<SnappMarketController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public SnappMarketController(ILogger<SnappMarketController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        //public async Task<IActionResult> AckOrder(int MarketPlaceOrderId)
        //{
        //    try
        //    {
        //        var message = await _unitOfWork.SnappMarketRepository.ChangeSnappMarketStatusApi(MarketPlaceOrderId, MarketPlaceStatusEnum.Ack, null);
        //        await _unitOfWork.SnappMarketRepository.ChangeStatusAsync(MarketPlaceStatusEnum.Ack, MarketPlaceOrderId, message);
        //        if (message == null)
        //            return Ok();
        //        return BadRequest(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return BadRequest(ex.Message);
        //    }
        //}

        [Route("OrderPick/{MarketPlaceOrderId}")]
        [HttpGet]
        public async Task<IActionResult> PickOrder([FromRoute] int MarketPlaceOrderId)
        {
            try
            {
                var message = await _unitOfWork.SnappMarketRepository.ChangeSnappMarketStatusApi(MarketPlaceOrderId, MarketPlaceStatusEnum.Pick, null);
                await _unitOfWork.SnappMarketRepository.ChangeStatusAsync(MarketPlaceStatusEnum.Pick, MarketPlaceOrderId, message);
                if (message == null)
                    return Ok();
                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("OrderAccept/{MarketPlaceOrderId}")]
        [HttpGet]
        public async Task<IActionResult> AcceptOrder([FromRoute] int MarketPlaceOrderId)
        {
            try
            {
                var message = await _unitOfWork.SnappMarketRepository.ChangeSnappMarketStatusApi(MarketPlaceOrderId, MarketPlaceStatusEnum.Accept, null);
                await _unitOfWork.SnappMarketRepository.ChangeStatusAsync(MarketPlaceStatusEnum.Accept, MarketPlaceOrderId, message);
                if (message == null)
                    return Ok();
                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("OrderReject")]
        [HttpPost]
        public async Task<IActionResult> RejectOrder([FromBody] MarketPlaceRejectDto rejectDto)
        {
            try
            {
                var message = await _unitOfWork.SnappMarketRepository.ChangeSnappMarketStatusApi(rejectDto.MarketPlaceOrderId, MarketPlaceStatusEnum.Reject, rejectDto);
                await _unitOfWork.SnappMarketRepository.ChangeStatusAsync(MarketPlaceStatusEnum.Reject, rejectDto.MarketPlaceOrderId, message);
                if (message == null)
                    return Ok();
                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [Route("api/MarketPlace/SnappMarket/AddOrUpdateProduct")]
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateProductAsync([FromBody] SnappMarketAddOrUpdateProductDto dto)
        {
            try
            {

                var response = await _unitOfWork.SnappMarketRepository.AddOrUpdateProductAsync(dto);
                if (response.IsSuccess)
                    return Ok();
                return BadRequest(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("api/MarketPlace/SnappMarket/StockUpdate")]
        [HttpPost]
        public async Task<IActionResult> StockUpdateAsync([FromBody] SnappMarketInventorySync dto)
        {
            try
            {

                var response = await _unitOfWork.SnappMarketRepository.StockUpdateAsync(dto);
                if (response.IsSuccess)
                    return Ok();
                return BadRequest(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
