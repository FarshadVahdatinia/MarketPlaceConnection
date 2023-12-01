using GPOS.Core.Entities;
using GPOS.Core.Enums;
using GPOS.Core.Helper;
using GPOS.Infrastructure.UOW;
using GPOS.MarketPlaceApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GPOS.MarketPlaceApi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class MarketPlaceController : ControllerBase
    {
        private readonly ILogger<MarketPlaceController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public MarketPlaceController(ILogger<MarketPlaceController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("SnappMarket")
        [HmacAuthorization]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> SnappMarket([FromForm] IFormCollection dto)
        {
            try
            {
                var order = _unitOfWork.SnappMarketRepository.SnappMarketInitialOrder(dto);
                var id = await _unitOfWork.SnappMarketRepository.AddAsync(order);
                try
                {
                    if (id == -1)
                    {
                        //وقتی کالای سفارش در دیتا بیس موجود نباشد
                        _logger.LogError($"Snappmarket Barcode and Database Barcode Does Not Match-OrderCode:{order?.OrderCode} - StatusCode:{order?.StatusCode}...Canceling Request");
                        if (order?.StatusCode == "51" || order?.StatusCode == "54" || order?.StatusCode == "71")
                            return Ok("Success");
                        await _unitOfWork.SnappMarketRepository.DirectRejectOrder(order.OrderCode);
                        return Ok("Success");
                    }
                    if (id > 0)
                    {
                        //سفارش جدید
                        var message = await _unitOfWork.SnappMarketRepository.ChangeSnappMarketStatusApi(id, MarketPlaceStatusEnum.Ack, null);
                        await _unitOfWork.SnappMarketRepository.ChangeStatusAsync(MarketPlaceStatusEnum.Ack, id, message);
                    }

                }
                catch (Exception) { }
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
