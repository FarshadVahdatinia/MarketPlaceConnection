namespace GW.CashAPI.Controllers
{

    public class MarketPlaceController : ApiController
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private SnappMarketService service;
        public MarketPlaceController()
        {
            service = new SnappMarketService();
        }
        [Route("api/MarketPlace/SnappMarket/AddOrUpdateProduct")]
        [HttpPost]
        public async Task<IHttpActionResult> AddOrUpdateProductAsync([FromBody] SnappMarketAddOrUpdateProductDto dto)
        {
            try
            {

                var response = await service.AddOrUpdateProductAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("api/MarketPlace/SnappMarket/StockUpdate")]
        [HttpPost]
        public async Task<IHttpActionResult> StockUpdateAsync([FromBody] SnappMarketInventorySync dto)
        {
            try
            {
                var response = await service.StockUpdateAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("api/MarketPlace/SnappMarket/OrderGet")]
        [HttpPost]
       // [HmacAuthorization]
        public async Task<IHttpActionResult> OrderGetAsync([FromBody] FormDataCollection dto)
        {
            try
            {
                var order = service.SnappMarketInitialOrder(dto);
                var id = await service.AddAsync(order);
                if (id == -1)
                {
                    //وقتی کالای سفارش در دیتا بیس موجود نباشد
                    await service.DirectRejectOrder(order);
                    return Ok("Success");
                }
                if ((id != 0 && id != -1) || order.StatusCode=="714")
                {
                    //سفارش تکراری
                    await service.AckOrderAsync(id);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return BadRequest(ex.Message.ToString());
            }
        }

        [Route("api/MarketPlace/SnappMarket/OrderPick")]
        [HttpGet]
        public async Task<IHttpActionResult> OrderPickAsync([FromUri] int marketPlaceOrderId)
        {
            try
            {
                var pickApi = await service.PickOrderAsync(marketPlaceOrderId);
                return Ok(pickApi);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return BadRequest(ex.Message.ToString());
            }
        }

        [Route("api/MarketPlace/SnappMarket/OrderAccept")]
        [HttpPost]
        public async Task<IHttpActionResult> OrderAcceptAsync([FromBody] AcceptOrderDto dto ,[FromUri] int marketPlaceOrderId)
        {
            try
            {
                var pickApi = await service.AcceptOrderAsync(dto, marketPlaceOrderId);
                return Ok(pickApi);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return BadRequest(ex.Message.ToString());
            }
        }

        [Route("api/MarketPlace/SnappMarket/RejectOrder")]
        [HttpPost]
        public async Task<IHttpActionResult> RejectOrderAsync([FromBody] MarketPlaceRejectDto dto)
        {
            try
            {
                var rejectApi = await service.RejectOrderAsync(dto);
                return Ok(rejectApi);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
