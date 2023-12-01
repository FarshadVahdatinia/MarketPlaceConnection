using Dapper;
using GPOS.Core.Contract;
using GPOS.Core.Dtos;
using GPOS.Core.Entities;
using GPOS.Core.Enums;
using GPOS.Core.Helper;
using GPOS.Infrastructure.Context;
using GPOS.Infrastructure.Dtos.SnappMarket;
using GPOS.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using System.Text;
using System.Transactions;

namespace GPOS.Infrastructure.Repository
{
    public class SnappMarketRepository : ISnappMarketRepository
    {
        private readonly GPOSDbContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly IConfiguration _configuration;

        public SnappMarketRepository(GPOSDbContext dbContext, ILogger<UnitOfWork> logger, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        #region OrderGet
        public MarketPlaceOrder SnappMarketInitialOrder(IFormCollection dto)
        {
            _logger.LogInformation("Received New Order From SnappMarket . . . Trying To fill properties");
            var marketPlaceOrder = new MarketPlaceOrder();
            IDictionary<string, string> products = new Dictionary<string, string>();
            IDictionary<string, string> toppings = new Dictionary<string, string>();
            List<KeyValuePair<string, string>> productKeyValue = null;
            marketPlaceOrder.MarketPlaceTypeId = (int)MarketPlaceTypeEnum.SnappMarket;
            foreach (var item in dto)
            {
                if (!item.Key.Contains("products["))
                {
                    switch (item.Key)
                    {
                        case "code":
                            marketPlaceOrder.OrderCode = item.Value;
                            continue;
                        case "userCode":
                            continue;
                        case "fullName":
                            marketPlaceOrder.FullName = item.Value;
                            continue;
                        case "firstName":
                            marketPlaceOrder.FirstName = item.Value;
                            continue;
                        case "lastName":
                            marketPlaceOrder.LastName = item.Value;
                            continue;
                        case "userAddressCode":
                            continue;
                        case "phone":
                            marketPlaceOrder.Phone = item.Value;
                            continue;
                        case "price":
                            marketPlaceOrder.Amount = (long)(Convert.ToDecimal(item.Value));
                            continue;
                        case "comment":
                            marketPlaceOrder.Comment = item.Value;
                            continue;
                        case "deliverAddress":
                            marketPlaceOrder.DeliverAddress = item.Value;
                            continue;
                        case "statusCode":
                            marketPlaceOrder.StatusCode = item.Value;
                            continue;
                        case "orderDate":
                            marketPlaceOrder.OrderDate = (long)(Convert.ToDecimal(item.Value));
                            continue;
                        case "latitude":
                            marketPlaceOrder.Latitude = item.Value;
                            continue;
                        case "longitude":
                            marketPlaceOrder.Longitude = item.Value;
                            continue;
                        case "deliveryPrice":
                            marketPlaceOrder.DeliveryPrice = (string.IsNullOrEmpty(item.Value) || item.Value == "null") ? 0 : Convert.ToInt32(item.Value);
                            continue;
                        case "packingPrice":
                            marketPlaceOrder.PackingPrice = (string.IsNullOrEmpty(item.Value) || item.Value == "null") ? 0 : Convert.ToInt32(item.Value);
                            continue;
                        case "deliveryTime":
                            marketPlaceOrder.DeliveryTime = (string.IsNullOrEmpty(item.Value) || item.Value == "null") ? 0 : Convert.ToInt32(item.Value);
                            continue;
                        case "preparationTime":
                            marketPlaceOrder.PreparationTime = (string.IsNullOrEmpty(item.Value) || item.Value == "null") ? 0 : Convert.ToInt32(item.Value);
                            continue;
                        case "taxCoeff":
                            marketPlaceOrder.TaxCoeff = (string.IsNullOrEmpty(item.Value) || item.Value == "null") ? 0 : Convert.ToInt32(item.Value);
                            continue;
                        case "tax":
                            marketPlaceOrder.Tax = (string.IsNullOrEmpty(item.Value) || item.Value == "null") ? 0 : Convert.ToInt32(item.Value);
                            continue;
                        case "vat":
                            marketPlaceOrder.Vat = (string.IsNullOrEmpty(item.Value) || item.Value == "null") ? 0 : Math.Round(Convert.ToDouble(item.Value), 2);
                            continue;
                        case "expeditionType":
                            marketPlaceOrder.ExpeditionType = item.Value;
                            continue;
                        case "discountType":
                            marketPlaceOrder.DiscountType = item.Value;
                            continue;
                        case "discountValue":
                            marketPlaceOrder.DiscountValue = (string.IsNullOrEmpty(item.Value) || item.Value == "null") ? 0 : Convert.ToInt32(item.Value);
                            continue;
                        case "newOrderDate":
                            marketPlaceOrder.NewOrderDate = Convert.ToDateTime((item.Value).ToString().Replace('+', ' '));
                            continue;
                        case "PreOrderTime":
                            marketPlaceOrder.DeliveryDueDateTime = Convert.ToDateTime((item.Value).ToString().Replace('+', ' '));
                            continue;
                        case "orderPaymentTypeCode":
                            marketPlaceOrder.OrderPaymentTypeCode = item.Value;
                            continue;
                        case "vendorMaxPreparationTime":
                            continue;
                        case "vendorCode":
                            marketPlaceOrder.VendorCode = item.Value;
                            continue;
                        case "couponDiscountSfShareAmount ":
                            continue;
                        case "couponDiscountVendorShareAmount":
                            continue;
                        default:
                            break;
                    }
                }

                if (item.Key.Contains("products[") && !item.Key.Contains("toppings]") && !item.Key.Contains("cuppon]"))
                {
                    products.Add(item.Key, item.Value);
                }
            }

            for (var i = 0; i < products.Where(x => x.Key.ToLower().Contains("[barcode]")).Count(); i++)
            {
                var product = new MarketPlaceOrderDetail();
                productKeyValue = null;
                productKeyValue = products.Where(x => x.Key.Contains($"[{i}]")).ToList();
                if (productKeyValue.Any())
                {
                    foreach (var x in productKeyValue)
                    {
                        switch (x.Key.ToString().Split('[')[2].ToLower().TrimEnd(']'))
                        {
                            case "id":
                                product.GoodId = (string.IsNullOrEmpty(x.Value) || x.Value == "null") ? 0 : Convert.ToInt32(x.Value);
                                continue;
                            case "quantity":
                                product.Quantity = (string.IsNullOrEmpty(x.Value) || x.Value == "null") ? 0 : Convert.ToDecimal(x.Value);
                                continue;
                            case "price":
                                product.Price = (string.IsNullOrEmpty(x.Value) || x.Value == "null") ? 0 : Convert.ToDecimal(x.Value);
                                continue;
                            case "originprice":
                                product.Price = (string.IsNullOrEmpty(x.Value) || x.Value == "null") ? 0 : Convert.ToDecimal(x.Value);
                                continue;
                            case "title":
                                product.title = x.Value;
                                continue;
                            case "productdiscountsfshare":
                                product.DiscountMPShare = (string.IsNullOrEmpty(x.Value) || x.Value == "null") ? 0 : Convert.ToDecimal(x.Value);
                                continue;
                            case "productdiscountvendorshare":
                                product.Discount = (string.IsNullOrEmpty(x.Value) || x.Value == "null") ? 0 : Convert.ToDecimal(x.Value);
                                continue;
                            case "discount":
                                product.Discount = (string.IsNullOrEmpty(x.Value) || x.Value == "null") ? 0 : int.Parse(x.Value);
                                continue;
                            case "vat":
                                product.VatPercent = (string.IsNullOrEmpty(x.Value) || x.Value == "null") ? 0 : (Convert.ToDecimal(x.Value) * 100);
                                continue;
                            case "barcode":
                                product.Barcode = x.Value;
                                continue;
                            default:
                                break;
                        }
                    }
                    marketPlaceOrder.MarketPlaceOrderDetails.Add(product);
                }
            }
            return marketPlaceOrder;
        }
        public async Task<int> AddAsync(MarketPlaceOrder entity)
        {
            _logger.LogInformation("Add New MarketPlace Order To MarketplaceOrder");
            var orderDetail = new DataTable();
            orderDetail.Columns.Add("GoodId");
            orderDetail.Columns.Add("Barcode");
            orderDetail.Columns.Add("Quantity");
            orderDetail.Columns.Add("Price");
            orderDetail.Columns.Add("PriceVat");
            orderDetail.Columns.Add("DiscountPercent");
            orderDetail.Columns.Add("Discount");
            orderDetail.Columns.Add("DiscountMPShare");
            orderDetail.Columns.Add("VatPercent");
            orderDetail.Columns.Add("ParentId");
            orderDetail.Columns.Add("title");
            orderDetail.Columns.Add("Comment");
            foreach (var item in entity.MarketPlaceOrderDetails)
            {
                orderDetail.Rows.Add(item.GoodId, item.Barcode, item.Quantity, item.Price, item.PriceVat
                    , item.DiscountPercent, item.Discount, item.DiscountMPShare, item.VatPercent, item.ParentId, item.title, item.Comment);
                _logger.LogInformation($"Order Status:{entity.StatusCode},ItemId{item.GoodId},Order Code:{entity.OrderCode} , , Customer :{entity.FullName},Quantity {item.Quantity},Price {item.Price}, PriceVat{item.PriceVat},DiscountPercent{item.DiscountPercent},Discount {item.Discount},VatPercent {item.VatPercent},Title {item.title}, Comment{item.Comment}");
            }

            var text = "salMarketPlaceOrderAdd";
            var param = new DynamicParameters();
            param.Add("@MarketPlaceTypeId", entity.MarketPlaceTypeId);
            param.Add("@VendorCode", entity.VendorCode);
            param.Add("@OrderCode", entity.OrderCode);
            param.Add("@OrderDate", entity.OrderDate);
            param.Add("@NewOrderDate", entity.NewOrderDate);
            param.Add("@StatusCode", entity.StatusCode);
            param.Add("@ExpeditionType", entity.ExpeditionType);
            param.Add("@FullName", entity.FullName);
            param.Add("@FirstName", entity.FirstName);
            param.Add("@LastName", entity.LastName);
            param.Add("@Latitude", entity.Latitude);
            param.Add("@Longitude", entity.Longitude);
            param.Add("@DeliverAddress", entity.DeliverAddress);
            param.Add("@Comment", entity.Comment);
            param.Add("@Phone", entity.Phone);
            param.Add("@Amount", entity.Amount);
            param.Add("@Tax", entity.Tax);
            param.Add("@DeliveryPrice", entity.DeliveryPrice);
            param.Add("@PackingPrice", entity.PackingPrice);
            param.Add("@DeliveryTime", entity.DeliveryTime);
            param.Add("@PreparationTime", entity.PreparationTime);
            param.Add("@Vat", entity.Vat, DbType.Double);
            param.Add("@TaxCoeff", entity.TaxCoeff);
            param.Add("@DiscountType", entity.DiscountType);
            param.Add("@DiscountValue", entity.DiscountValue);
            param.Add("@OrderPaymentTypeCode", entity.OrderPaymentTypeCode);
            param.Add("@OrderPayment", entity.OrderPayment);
            param.Add("@DeliveryDueDateTime", entity.DeliveryDueDateTime);
            param.Add("@tvpMarketPlaceOrderDetail", orderDetail.AsTableValuedParameter("MarketPlaceOrderDetailType"));
            param.Add("@Id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var cnn = _dbContext.Database.GetDbConnection();
                _logger.LogInformation($"MarketPlaceTypeId:{entity.MarketPlaceTypeId}, VendorCode:{entity.VendorCode}, OrderCode:{entity.OrderCode}, OrderDate:{entity.OrderDate}, NewOrderDate:{entity.NewOrderDate},StatusCode: {entity.StatusCode},ExpeditionType: {entity.ExpeditionType},FullName: {entity.FullName}, FirstName:{entity.FirstName}, LastName:{entity.LastName},Latitude: {entity.Latitude},Longitude: {entity.Longitude}, DeliverAddress:{entity.DeliverAddress}, Comment:{entity.Comment},Phone: {entity.Phone},Amount: {entity.Amount}, Tax:{entity.Tax},DeliveryPrice: {entity.DeliveryPrice},PackingPrice: {entity.PackingPrice}, DeliveryTime:{entity.DeliveryTime},PreparationTime: {entity.PreparationTime},Vat: {entity.Vat},TaxCoeff: {entity.TaxCoeff}, DiscountType:{entity.DiscountType},DiscountValue: {entity.DiscountValue},OrderPaymentTypeCode: {entity.OrderPaymentTypeCode},OrderPayment: {entity.OrderPayment},DeliveryDueDateTime: {entity.DeliveryDueDateTime}  ");
                _logger.LogInformation("Execute salMarketPlaceOrderAdd");
                await cnn.ExecuteAsync(text, param, commandType: System.Data.CommandType.StoredProcedure);
                scope.Complete();

            }
            var res = param.Get<int>("@Id");
            if (res != 0 && res != -1)
                if (!await _dbContext.MarketPlaceOrders.AnyAsync(x => x.Id == res))
                    throw new Exception("اضافه کردن سفارش با مشکل مواجه شد");
            return res;
        }
        public async Task<string?> ChangeSnappMarketStatusApi(int marketPlaceOrderId, MarketPlaceStatusEnum marketPlaceStatus, MarketPlaceRejectDto? rejectDto)
        {

            var marketPlaceOrder = await _dbContext.MarketPlaceOrders.FirstOrDefaultAsync(x => x.Id == marketPlaceOrderId && x.MarketPlaceTypeId == (int)MarketPlaceTypeEnum.SnappMarket) ?? throw new Exception($" با کد سفارش {marketPlaceOrderId} یافت نشد");
            var isChengedBefore = await IsStatusChangedByCustomer(marketPlaceStatus, marketPlaceOrder);
            if (isChengedBefore.isChanged)
            {
                return isChengedBefore.message;
            }
            var json = CreateSnappDtoToSend(marketPlaceStatus, marketPlaceOrder, rejectDto);
            var address = UrlMaker(marketPlaceStatus, marketPlaceOrder.OrderCode);
            _logger.LogInformation($"Token From Database - status {marketPlaceStatus} OrderCode {marketPlaceOrder?.OrderCode}");
            var token = await GetSnappMarketTokenFromDb();
            var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json, token, RequestTypeEnum.Post);
            var hasErrorCode = await ErrorCodeCheckAsync(marketPlaceOrder, response.codeErrorMessage);
            if (hasErrorCode.hasCode && (hasErrorCode.ErrorCode >= 3001 && hasErrorCode.ErrorCode <= 3010))
            {
                _logger.LogWarning($"Token UnAuthorized For OrderId:{marketPlaceOrder?.Id} Code:{marketPlaceOrder?.OrderCode} - Get Token From snappMarket");
                var newToken = await GetToken();
                response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json
                , token, RequestTypeEnum.Post);
                hasErrorCode = await ErrorCodeCheckAsync(marketPlaceOrder, response.codeErrorMessage);
            }
            if (hasErrorCode.hasCode && hasErrorCode.ErrorCode == 1061)
            {
                _logger.LogWarning($"Reject Reason was Not Acceptable for SnappMarket for  OrderId:{marketPlaceOrder?.Id} Code:{marketPlaceOrder?.OrderCode} - Send Again With another Reject Reason Id");
                rejectDto.RejectReasonId = 119;
                var newRejectDto = CreateSnappDtoToSend(marketPlaceStatus, marketPlaceOrder, rejectDto);
                response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json, token, RequestTypeEnum.Post);
                hasErrorCode = await ErrorCodeCheckAsync(marketPlaceOrder, response.codeErrorMessage);
                if (hasErrorCode.hasCode && (hasErrorCode.ErrorCode >= 3001 && hasErrorCode.ErrorCode <= 3010))
                {
                    _logger.LogWarning($"Token UnAuthorized For OrderId:{marketPlaceOrder?.Id} Code:{marketPlaceOrder?.OrderCode} - Get Token From snappMarket");
                    var newToken = await GetToken();
                    response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json, token, RequestTypeEnum.Post);
                }
            }
            return response.response?.ErrorMessage;
        }
        private string? CreateSnappDtoToSend(MarketPlaceStatusEnum marketPlaceStatus, MarketPlaceOrder order, MarketPlaceRejectDto? rejectDto)
        {
            string? json = null;
            switch (marketPlaceStatus)
            {
                case MarketPlaceStatusEnum.Ack:
                    break;
                case MarketPlaceStatusEnum.Pick:
                    break;
                case MarketPlaceStatusEnum.Accept:
                    _logger.LogInformation($"Accepting Order . . . order:{order.OrderCode}");
                    json = JsonConvert.SerializeObject(new
                    {
                        packingPrice = order.PackingPrice ?? Convert.ToInt32(_configuration["Snapp:snappMarketPackingPrice"]),
                        delta = Convert.ToInt32(_configuration["Snapp:snappMarketDelta"]),
                        deliveryTime = order.ExpeditionType.ToLower() == "delivery" ? (order.DeliveryTime ?? Convert.ToInt32(_configuration["Snapp:snappMarketDeliveryTime"])) : 1,
                        riderPickupTime = order.ExpeditionType.ToLower() != "delivery" ? (order?.PreparationTime ?? Convert.ToInt32(_configuration["Snapp:snappMarketRiderPickupTime"])) : 1,
                    });
                    break;
                case MarketPlaceStatusEnum.Reject:
                    _logger.LogWarning($"Rejecting Order . . .  order:{order.OrderCode} RejectReasonId:{rejectDto?.RejectReasonId}");

                    if (rejectDto.NonExistentProducts == null || !rejectDto.NonExistentProducts.Any())
                    {
                        rejectDto.NonExistentProducts = new List<NonExistentProduct> { new NonExistentProduct() { barcode = "111", suggestedProductBarcodes = new List<string> { "1", "2", "3", } } };
                    }

                    var nonExistentProducts = new List<NonExistentProduct>();
                    rejectDto.NonExistentProducts.ForEach(x =>
                    {
                        var snappFoodNonExistentProducts = new NonExistentProduct() { barcode = x.barcode, suggestedProductBarcodes = x.suggestedProductBarcodes };
                        nonExistentProducts.Add(snappFoodNonExistentProducts);
                    });

                    var rejectJson = new
                    {
                        reasonId = rejectDto.RejectReasonId,
                        comment = rejectDto.Comment,
                        nonExistentProducts = rejectDto.NonExistentProducts
                    };
                    json = JsonConvert.SerializeObject(rejectJson);
                    break;
                default:
                    break;
            }
            return json;
        }
        #endregion

        #region StatusCheck
        private async Task<(bool hasCode, int? ErrorCode)> ErrorCodeCheckAsync(MarketPlaceOrder? marketPlaceOrder, string? errors, bool isOrderRequest = true)
        {
            bool hasCode = false;
            int? errorCode = null;
            if (string.IsNullOrEmpty(errors))
                return (hasCode, errorCode);
            var error = JsonConvert.DeserializeObject<SnappMarketErrorJsonModel>(errors);
            var code = error?.error?.Code;
            if (code != null)
            {
                if (code >= 3001 && code <= 3010)
                {
                    hasCode = true;
                    errorCode = code;
                    _logger.LogError("توکن قابلیت احراز ندارد");
                    return (hasCode, errorCode);
                }
                if (isOrderRequest)
                {
                    switch (code)
                    {
                        case 1023:
                            //سفارش یافت نشد
                            marketPlaceOrder.DocStatusId = (int)DocStatusEnum.Canceled;
                            marketPlaceOrder.IsActive = false;
                            hasCode = true;
                            errorCode = code;
                            break;
                        case 1068:
                            //سفارش قبلا پیک شده است
                            marketPlaceOrder.AckApiOn ??= DateTime.Now;
                            marketPlaceOrder.PickApiOn ??= DateTime.Now;
                            marketPlaceOrder.PickApiError = error?.AllErrorsInString;
                            hasCode = true;
                            errorCode = code;
                            break;
                        case 1059:
                            //سفارش قبلا تایید شده
                            marketPlaceOrder.AckApiOn ??= DateTime.Now;
                            marketPlaceOrder.PickApiOn ??= DateTime.Now;
                            marketPlaceOrder.AcceptApiOn ??= DateTime.Now;
                            marketPlaceOrder.DocStatusId = (int)DocStatusEnum.Confirm;
                            marketPlaceOrder.IsActive = false;
                            marketPlaceOrder.AcceptApiError = error?.AllErrorsInString;
                            hasCode = true;
                            errorCode = code;
                            break;
                        case 1060:
                            //سفارش قبلا ریجکت شده
                            marketPlaceOrder.AckApiOn ??= DateTime.Now;
                            marketPlaceOrder.PickApiOn ??= DateTime.Now;
                            marketPlaceOrder.RejectApiOn ??= DateTime.Now;
                            marketPlaceOrder.DocStatusId = (int)DocStatusEnum.Canceled;
                            marketPlaceOrder.IsActive = false;
                            marketPlaceOrder.RejectApiError = error?.AllErrorsInString;
                            hasCode = true;
                            errorCode = code;
                            break;
                        case 1061:
                            //Reject Reason Could not be find
                            hasCode = true;
                            errorCode = code;
                            break;
                        default:
                            break;
                    }
                    if (hasCode == true)
                    {
                        _logger.LogError($"{error?.AllErrorsInString} for order : {marketPlaceOrder.OrderCode} ");
                        _dbContext.MarketPlaceOrders.Update(marketPlaceOrder);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            return (hasCode, errorCode);
        }
        public async Task ChangeStatusAsync(MarketPlaceStatusEnum phaseApi, int marketPlaceOrderId, string? error, int? rejectReasonId = null)
        {
            _logger.LogInformation($"ChangeStatus Called with Status {phaseApi} for marketplace:{marketPlaceOrderId} and error:{error}");
            var order = await _dbContext.MarketPlaceOrders.FirstOrDefaultAsync(x => x.Id == marketPlaceOrderId && x.MarketPlaceTypeId == (int)MarketPlaceTypeEnum.SnappMarket) ?? throw new CatchedByMeException("سفارش یافت نشد");
            switch (phaseApi)
            {
                case MarketPlaceStatusEnum.Ack:
                    if (string.IsNullOrEmpty(error))
                    {
                        order.AckApiOn ??= DateTime.Now;
                    }
                    else
                    {
                        order.AckApiErrorOn = DateTime.Now;
                        order.AckApiError = error;
                    }
                    break;
                case MarketPlaceStatusEnum.Pick:
                    if (string.IsNullOrEmpty(error))
                    {
                        order.PickApiOn ??= DateTime.Now;
                    }
                    else
                    {
                        order.PickApiErrorOn = DateTime.Now;
                        order.PickApiError = error;
                    }
                    break;
                case MarketPlaceStatusEnum.Accept:
                    if (string.IsNullOrEmpty(error))
                    {
                        order.AcceptApiOn ??= DateTime.Now;
                        order.IsActive = false;
                        order.DocStatusId = (int)DocStatusEnum.Confirm;
                    }
                    else
                    {
                        order.AcceptApiErrorOn = DateTime.Now;
                        order.AcceptApiError = error;
                    }
                    break;

                case MarketPlaceStatusEnum.Reject:
                    if (string.IsNullOrEmpty(error))
                    {
                        order.RejectApiOn ??= DateTime.Now;
                        order.IsActive = false;
                        order.DocStatusId = (int)DocStatusEnum.Canceled;
                        order.RejectReason = Convert.ToString(rejectReasonId ?? 119);

                    }
                    else
                    {
                        order.RejectApiErrorOn = DateTime.Now;
                        order.RejectApiError = error;
                    }
                    break;
                default:
                    break;
            }

            _dbContext.MarketPlaceOrders.Update(order);
            await _dbContext.SaveChangesAsync();
        }
        private async Task<(bool isChanged, string? message)> IsStatusChangedByCustomer(MarketPlaceStatusEnum phaseApi, MarketPlaceOrder order)
        {
            //این متد برای مواقعی است که قبل از صندوق دار کاربر تغییراتی را روی سفارش اعمال کرده باشد
            if (order == null)
            {
                throw new CatchedByMeException("سفارش یافت نشد");
            }
            string msg = "";
            bool hasChanged = false;
            if (order.RejectApiOn != null && order.DocStatusId == (int)DocStatusEnum.Canceled)
            {
                msg = $" سفارش اسنپ مارکت{order?.OrderCode} کنسل شده است";
                _logger.LogError(msg);
                return (true, msg);
            }
            if (order.StatusCode == "51" || order.StatusCode == "71" || order.StatusCode == "54")
            {
                order.RejectApiOn ??= DateTime.Now;
                order.IsActive = false;
                order.DocStatusId = (int)DocStatusEnum.Canceled;
                msg = $" سفارش اسنپ مارکت{order?.OrderCode} کنسل شده است";
                order.RejectApiError = msg;
                hasChanged = true;
                _logger.LogError(msg);
                return (true, msg);
            }
            switch (phaseApi)
            {

                case MarketPlaceStatusEnum.Ack:
                    if (hasChanged)
                        break;
                    if (order.AckApiOn != null)
                    {
                        msg = $"OrderCode:{order?.OrderCode}وضعیت سفارش قبلا تغییر کرده است";
                        _logger.LogError(msg);
                        hasChanged = true;
                    }
                    if (order.StatusCode != "714" && order.StatusCode != "56")
                    {
                        msg = $"OrderCode:{order?.OrderCode}وضعیت سفارش قبلا تغییر کرده است";
                        order.AckApiOn ??= DateTime.Now;
                        hasChanged = true;
                    }
                    break;

                case MarketPlaceStatusEnum.Pick:
                    if (hasChanged)
                        break;
                    if (order.RejectApiOn != null || order.AcceptApiOn != null || order.PickApiOn != null || order.StatusCode == "713")
                    {
                        msg = $"OrderCode:{order?.OrderCode} وضعیت سفارش قبلا تغییر کرده است";
                        order.PickApiOn ??= DateTime.Now;
                        hasChanged = true;
                    }
                    break;
                case MarketPlaceStatusEnum.Accept:

                    if (hasChanged)
                        break;
                    if (((order.AcceptApiOn != null || order.RejectApiOn != null) && order.DocStatusId != (int)DocStatusEnum.Draft) || !order.IsActive)
                    {
                        msg = $"وضعیت سفارش {order?.OrderCode} تغییر یافته است";
                        hasChanged = true;
                    }
                    if (order.StatusCode == "42")
                    {
                        order.AcceptApiOn ??= DateTime.Now;
                        order.IsActive = false;
                        order.DocStatusId = (int)DocStatusEnum.Confirm;
                        msg = $"سفارش {order?.OrderCode} قبلا تایید شده است";
                        hasChanged = true;
                    }
                    break;

                case MarketPlaceStatusEnum.Reject:
                    break;
                default:
                    break;
            }
            if (hasChanged)
            {
                _dbContext.MarketPlaceOrders.Update(order);
                await _dbContext.SaveChangesAsync();
                _logger.LogError(msg);
                return (true, msg);
            }
            return (false, null);

        }
        public async Task DirectRejectOrder(string orderCode)
        {
            var address = UrlMaker(MarketPlaceStatusEnum.Reject, orderCode);
            var token = await GetToken() ?? throw new Exception("توکن دریافت نگردید");
            var json = JsonConvert.SerializeObject(new
            {
                comment = "آیدی یا بارکد با منو هم خوانی ندارد",
                reasonId = 119,
            });
            _logger.LogError($"Direct Cancel Order=> {orderCode}");
            var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json, token, RequestTypeEnum.Post);

            if (response.response.IsSuccess)
            {
                _logger.LogInformation($"Order:{orderCode} has been Rejected successfully due to mismatch of Barcodes from snappmarket and Database");
            }
            else
            {
                _logger.LogError($"Order:{orderCode} has Not been Rejected successfully due to mismatch of Barcodes from snappmarket and Database");
            }
        }
        #endregion

        #region ConnectToSnapp
        private Uri UrlMaker(MarketPlaceStatusEnum snappApiAddressEnum, string? orderCode = null)
        {
            var marketPlaceType = _dbContext.MarketPlaceTypes.Where(x => x.Code == ((int)MarketPlaceTypeEnum.SnappMarket).ToString())?.FirstOrDefault();
            var snappMarketBaseAddress = marketPlaceType.Address;
            var tokenAddress = marketPlaceType.TokenAddress;

            var apiAddress = snappApiAddressEnum switch
            {
                MarketPlaceStatusEnum.Ack => snappMarketBaseAddress + marketPlaceType.AckAPI.Replace("{order_code}", orderCode),
                MarketPlaceStatusEnum.Pick => snappMarketBaseAddress + marketPlaceType.PickAPI.Replace("{order_code}", orderCode),
                MarketPlaceStatusEnum.Accept => snappMarketBaseAddress + marketPlaceType.AcceptAPI.Replace("{order_code}", orderCode),
                MarketPlaceStatusEnum.Reject => snappMarketBaseAddress + marketPlaceType.RejectAPI.Replace("{order_code}", orderCode),
                MarketPlaceStatusEnum.StockUpdate => snappMarketBaseAddress + "/va/v1/product/stock",
                MarketPlaceStatusEnum.ProductUpdate => snappMarketBaseAddress + "/va/v1/product/productDetails",
                MarketPlaceStatusEnum.Token => tokenAddress,
                _ => throw new Exception("آدرس انتخابی موجود نمی باشد"),
            };
            var url = new Uri(apiAddress);
            return url;
        }
        private async Task<string> GetToken()
        {
            var marketPlaceVendor = _dbContext.MarketPlaceVendors.Where(x => x.MarketPlaceTypeId == (int)MarketPlaceTypeEnum.SnappMarket)?.FirstOrDefault();
            string? userName = marketPlaceVendor.Username;
            string? password = marketPlaceVendor.Password;
            string? vendorCode = marketPlaceVendor.VendorCode;
            string? vendorSecret = marketPlaceVendor.VendorSecret;
            var address = UrlMaker(MarketPlaceStatusEnum.Token);
            string data = "username=" + userName + "&password=" + password + "&client_id=" + vendorCode + "&client_secret=" + vendorSecret + "&scope=automation&grant_type=password";
            byte[] dataStream = Encoding.UTF8.GetBytes(data);

            HttpWebRequest httpWebRequest;
            httpWebRequest = (HttpWebRequest)WebRequest.Create(address);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = dataStream.Length;
            Stream newStream = httpWebRequest.GetRequestStream();
            newStream.Write(dataStream, 0, dataStream.Length);
            newStream.Close();

            WebResponse httpResponse = httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var token = streamReader.ReadToEnd();
                JObject json = JObject.Parse(token);
                var accessToken = json["access_token"].ToString();

                var marketPLaceType = await _dbContext.MarketPlaceTypes
                .FirstOrDefaultAsync(c => c.Code == ((int)MarketPlaceTypeEnum.SnappMarket).ToString() && c.IsActive) ?? throw new Exception("وندور یافت نشد");
                marketPLaceType.TokenExpire = DateTime.Now.AddMinutes(Convert.ToInt32(string.IsNullOrEmpty(_configuration["Snapp:SnappMarketTokenExpireTimeInMin"]) ? "0" : _configuration["Snapp:SnappMarketTokenExpireTimeInMin"]));
                marketPLaceType.Token = accessToken;
                await _dbContext.SaveChangesAsync();
                var msg = string.IsNullOrWhiteSpace(accessToken) ? $"توکن دریافت نگردید {vendorCode}" : $"Get Token was successful for vendorCode : {vendorCode}";
                _logger.LogInformation(msg);
                return accessToken;
            }
        }
        private async Task<string> GetSnappMarketTokenFromDb()
        {
            string? token = string.Empty;
            var marketPLaceType = await _dbContext.MarketPlaceTypes.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == ((int)MarketPlaceTypeEnum.SnappMarket).ToString()
                    && c.IsActive);
            token = (marketPLaceType?.TokenExpire != null && ((DateTime)marketPLaceType.TokenExpire).CompareTo(DateTime.Now) < 0) ? marketPLaceType?.Token : string.Empty;

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogInformation($"Database token Was Not Acceptable - Getting Token From Snapp !");
                token = await GetToken();
            }

            return token;
        }
        private async Task<(MyResponse<TData> response, string? codeErrorMessage)> ConnectToSnappMarketAsync<TData>(Uri url, string? serializedJsonBody, string token, RequestTypeEnum requestTypeEnum) where TData : class
        {
            _logger.LogInformation($"Connect To SnappMarket And Sending Data --RequestType:{requestTypeEnum} --ApiAddress : {url} -- body: {serializedJsonBody} ");
            serializedJsonBody ??= JsonConvert.SerializeObject(serializedJsonBody);
            var content = new StringContent(serializedJsonBody, Encoding.UTF8, "application/json");
            MyResponse<TData> res = new();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = requestTypeEnum switch
                {
                    RequestTypeEnum.Post => await client.PostAsync(url, content),
                    RequestTypeEnum.Put => await client.PutAsync(url, content),
                    RequestTypeEnum.Get => await client.GetAsync(url),
                    _ => throw new Exception("متد ورودی یافت نشد"),
                };
                if (response.StatusCode == HttpStatusCode.NoContent && string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
                {
                    res.StatusCode = (int)HttpStatusCode.OK;
                    res.IsSuccess = true;
                    res.Data = null;
                    return (res, null);
                }
                if (response.StatusCode == HttpStatusCode.OK && string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
                {
                    var resp = JsonConvert.DeserializeObject<TData>(await response.Content.ReadAsStringAsync());
                    res.StatusCode = (int)HttpStatusCode.OK;
                    res.IsSuccess = true;
                    res.Data = resp;
                    return (res, null);
                }
                else
                {
                    string errorMsg = "";
                    string jsonError = "";
                    if (!string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
                    {
                        var resp = JsonConvert.DeserializeObject<SnappMarketErrorModel>(await response.Content.ReadAsStringAsync());
                        /// برمیگرداند هردو رو پر میکنیم error و هم errors به علت اینکه در مدل ارور هم 
                        var Message = resp?.Error?.Message;
                        var Code = resp?.Error?.Code.ToString();
                        var failedItems = resp?.failedItems?.FirstOrDefault();

                        StringBuilder sb = new StringBuilder();
                        var errorsDic = resp?.Errors?.ToList();
                        errorsDic?.ForEach(x =>
                        {
                            sb.Append(x.Key);
                            sb.Append(": ");
                            sb.AppendLine(x.Value);
                        });
                        errorMsg = $"Error snappMarket.StatusCode:{response.StatusCode}{Message + " " + sb?.ToString() + "SnappCode:" + Code ?? "null"} + failedItems:{failedItems ?? null}";
                        _logger.LogError(errorMsg);
                        jsonError = JsonConvert.SerializeObject(new SnappMarketErrorJsonModel()
                        {
                            AllErrorsInString = errorMsg,
                            error = resp?.Error
                        });
                    }
                    res.StatusCode = (int)HttpStatusCode.BadRequest;
                    res.ErrorMessage = errorMsg;
                    res.IsSuccess = false;
                    res.Data = null;
                    _logger.LogError(errorMsg);
                    return (res, jsonError);
                }
            }
        }
        #endregion

        #region ProductUpdate
        public async Task<MyResponse<SnappMarketNullResponseDto>> AddOrUpdateProductAsync(SnappMarketAddOrUpdateProductDto dto)
        {
            if (dto == null || !dto.products.Any())
                throw new CatchedByMeException("موارد ورودی کامل نیست");
            dto.products.ForEach(p => { _logger.LogInformation($"SnappMarket Pruduct Change.Barcode:{p.barcode}-price:{p.price}-vendor{dto.vendorCode}"); });
            var token = await GetSnappMarketTokenFromDb();
            var json = JsonConvert.SerializeObject(dto);
            var address = UrlMaker(MarketPlaceStatusEnum.ProductUpdate);
            var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json, token, RequestTypeEnum.Post);
            var hasErrorCode = await ErrorCodeCheckAsync(null, response.codeErrorMessage, false);
            if (hasErrorCode.hasCode && (hasErrorCode.ErrorCode >= 3001 && hasErrorCode.ErrorCode <= 3010))
            {
                var newToken = await GetToken();
                response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json
                , token, RequestTypeEnum.Post);
            }
            return response.response;
        }
        public async Task<MyResponse<SnappMarketNullResponseDto>> StockUpdateAsync(SnappMarketInventorySync dto)
        {
            if (dto == null || !dto.stocks.Any())
                throw new Exception("موارد ورودی کامل نیست");
            var token = await GetSnappMarketTokenFromDb();
            var json = JsonConvert.SerializeObject(dto);
            var address = UrlMaker(MarketPlaceStatusEnum.StockUpdate);
            dto.stocks.ForEach(p => { _logger.LogInformation($"SnappMarket Stock - barcode:{p.barcode} - Stock:{p.stock} - vendor{dto.vendorCode}"); });
            var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json, token, RequestTypeEnum.Post);
            var hasErrorCode = await ErrorCodeCheckAsync(null, response.codeErrorMessage, false);
            if (hasErrorCode.hasCode && (hasErrorCode.ErrorCode >= 3001 && hasErrorCode.ErrorCode <= 3010))
            {
                var newToken = await GetToken();
                response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json
                , token, RequestTypeEnum.Post);
            }
            return response.response;
        }
        #endregion

        #region ChangeStatusByWorker

        public async Task<List<MarketPlaceOrder>> ChangeMarketPlaceStatusWorker(List<MarketPlaceOrder> orders)
        {
            foreach (var order in orders)
            {
                MarketPlaceRejectDto? marketPlaceRejectDto = null;
                var marketPlaceStatus = await OrderStatusFind(order);
                if (marketPlaceStatus == null)
                {
                    //
                }
                if (marketPlaceStatus == MarketPlaceStatusEnum.Reject)
                {
                    var rejectreasonId = order.MarketPlaceOrderDetails.Any(x => x.RejectReasonId == 139 || x.RejectReasonId == 177) ? 139 : (order?.MarketPlaceOrderDetails?.FirstOrDefault(x => x.RejectReasonId != null)?.RejectReasonId ?? 119);

                    var nonExistantProducts = new List<NonExistentProduct>();
                    order.MarketPlaceOrderDetails.Where(x=> x.RejectReasonId == 139 || x.RejectReasonId == 177 ).ToList().ForEach(x =>
                    {
                        nonExistantProducts.Add(new NonExistentProduct()
                        {
                            barcode = x.Barcode,
                            suggestedProductBarcodes = x?.GoodsReplace?.Split(',')?.ToList() ?? new List<string>() { "1","2"}
                        });
                    });

                    marketPlaceRejectDto = new MarketPlaceRejectDto()
                    {
                        MarketPlaceOrderId = order.Id,
                        OrderCode = order.OrderCode,
                        RejectReasonId = rejectreasonId,
                        Comment = order.MarketPlaceOrderDetails.FirstOrDefault(x => x.Comment != null)?.Comment,
                        NonExistentProducts = nonExistantProducts
                    };
                }
                var message = await ChangeSnappMarketStatusApi(order.Id, marketPlaceStatus, marketPlaceRejectDto);
                await ChangeStatusAsync(marketPlaceStatus, order.Id, message);

            }
        }

        public async Task<MarketPlaceStatusEnum?> OrderStatusFind(MarketPlaceOrder order)
        {
            if (order.AcceptOn != null && order.DocStatusId == (int)DocStatusEnum.Confirm && order.AcceptApiErrorOn == null && order.AcceptApiOn == null && order.IsActive)
                return MarketPlaceStatusEnum.Accept;

            if (order.RejectOn != null && order.DocStatusId == (int)DocStatusEnum.Canceled && order.RejectApiErrorOn == null && order.RejectApiOn == null && order.IsActive)
                return MarketPlaceStatusEnum.Reject;

            if (order.PickOn != null && order.DocStatusId == (int)DocStatusEnum.Draft && order.PickApiErrorOn == null && order.PickApiOn == null)
                return MarketPlaceStatusEnum.Pick;

            return null;

        }
        #endregion


        #region OldCode
        //private async Task ChangeStatusByUser(SnappApiEnum phaseApi, MarketPlaceOrder marketPlaceOrder)
        //{
        //    switch (phaseApi)
        //    {

        //        case SnappApiEnum.PickApi:
        //            marketPlaceOrder.PickOn = DateTime.Now;
        //            //PickBy By Cashier it will fill by number 10
        //            marketPlaceOrder.PickBy = 10;
        //            break;
        //        case SnappApiEnum.AcceptApi:
        //            marketPlaceOrder.AcceptOn = DateTime.Now;
        //            //Accept By Cashier it will fill by number 10
        //            marketPlaceOrder.AcceptBy = 10;
        //            break;

        //        case SnappApiEnum.RejectApi:
        //            marketPlaceOrder.RejectOn = DateTime.Now;
        //            //if rejected By Cashier it fills by number 10
        //            marketPlaceOrder.RejectBy = 10;
        //            break;
        //        default:
        //            break;
        //    }

        //    _dbContext.MarketPlaceOrders.Update(marketPlaceOrder);
        //    await _dbContext.SaveChangesAsync();
        //}
        //public async Task DirectRejectOrder(MarketPlaceOrder order)
        //{
        //    if (order.StatusCode == "51" || order.StatusCode == "54" || order.StatusCode == "71")
        //        return;
        //    var address = UrlMaker(MarketPlaceStatusEnum.RejectApi, order.OrderCode);
        //    var token = GetToken() ?? throw new Exception("توکن دریافت نگردید");
        //    var json = JsonConvert.SerializeObject(new
        //    {
        //        comment = "آیدی یا بارکد با منو هم خوانی ندارد",
        //        reasonId = 119,
        //    });
        //    _logger.LogError($"Direct Cancel Order=> {JsonConvert.SerializeObject(order)}");
        //    var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(address, json, token, RequestTypeEnum.Post, null);
        //    if (response.response.IsSuccess)
        //        logger.Info($"order:{order?.OrderCode} successfully Canceled ");
        //    else
        //        logger.Error($"order:{order?.OrderCode} in Direct Reject Couldn't proceed Well and had error from snapp response:{response.response?.ErrorMessage} ");
        //}
        //public async Task<MyResponse<SnappMarketNullResponseDto>?> AckOrderAsync(int? marketPlaceOrderId)
        //{
        //    var marketPlaceOrder = await _dbContext.MarketPlaceOrders.FirstOrDefaultAsync(x => x.Id == marketPlaceOrderId && x.MarketPlaceTypeId == (int)MarketPlaceTypeEnum.SnappMarket) ?? throw new Exception($" با کد سفارش {marketPlaceOrderId} یافت نشد");
        //    var isChengedBefore = await IsStatusChangedByCustomer(MarketPlaceStatusEnum.Ack, marketPlaceOrder);
        //    if (isChengedBefore.isChanged)
        //    {
        //        throw new CatchedByMeException(isChengedBefore.message ?? "Error in SnappMarket ");
        //    }
        //    var token = await GetToken() ?? throw new Exception("توکن دریافت نگردید");
        //    var url = UrlMaker(SnappApiEnum.AckApi, marketPlaceOrder.OrderCode);

        //    var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(url, null
        //        , token, RequestTypeEnum.Post);
        //    var hasErrorCode = await ErrorCodeCheckAsync(marketPlaceOrder, response.codeErrorMessage);
        //    if (hasErrorCode.hasCode && (hasErrorCode.ErrorCode >= 3001 && hasErrorCode.ErrorCode <= 3010))
        //    {
        //        var newToken = await GetToken(dto.UserName, dto.Password, dto.VendorCode, dto.VendorSecret, dto.TokenAddress);
        //        response = await ConnectToSnappMarketAsync<SnappNullResponseDto>(address, json, newToken, HttpRequestEnum.Post);
        //        hasErrorCode = await SnappErrorCodeCheckAsync(order, response.codeErrorMessage);
        //    }
        //    if (hasErrorCode.hasCode && hasErrorCode.ErrorCode == 1061)
        //    {
        //        dto.ReasonId = "119";
        //        var newRejectDto = CreateSnappDtoToSend(dto, order);
        //        response = await ConnectToSnappMarketAsync<SnappNullResponseDto>(address, newRejectDto, pass, HttpRequestEnum.Post);
        //        hasErrorCode = await ErrorCodeCheckAsync(order, response.codeErrorMessage);
        //        if (hasErrorCode.hasCode && (hasErrorCode.ErrorCode >= 3001 && hasErrorCode.ErrorCode <= 3010))
        //        {
        //            var newToken = await GetToken(dto.UserName, dto.Password, dto.VendorCode, dto.VendorSecret, dto.TokenAddress);
        //            response = await ConnectToSnappMarketAsync<SnappNullResponseDto>(address, json, newToken, HttpRequestEnum.Post);
        //        }
        //    }
        //    return response.response?.ErrorMessage;
        //}
        //public async Task<MyResponse<SnappMarketNullResponseDto>?> PickOrderAsync(int marketPlaceOrderId)
        //{
        //    var marketPlaceOrder = await _dbContext.MarketPlaceOrders.FirstOrDefaultAsync(x => x.Id == marketPlaceOrderId && x.MarketPlaceTypeId == (int)MarketPlaceTypeEnum.SnappMarket) ?? throw new Exception($" با کد سفارش {marketPlaceOrderId} یافت نشد");
        //    if (marketPlaceOrder.PickApiOn != null)
        //        throw new Exception("وضعیت سفارش تغییر یافته است");
        //    if (marketPlaceOrder.StatusCode == "713")
        //    {
        //        await ChangeStatusAsync(SnappApiEnum.PickApi, marketPlaceOrder, true, null);
        //        throw new Exception("سفارش قبلا مشاهده شده است");
        //    }
        //    await ChangeStatusByUser(SnappApiEnum.PickApi, marketPlaceOrder);
        //    var token = await GetToken() ?? throw new Exception("توکن دریافت نگردید");
        //    var url = UrlMaker(SnappApiEnum.PickApi, marketPlaceOrder.OrderCode);
        //    var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(url, null, token, RequestTypeEnum.Post);
        //    var hasErrorCode = await ErrorCodeCheckAsync(marketPlaceOrder, response.codeErrorMessage);
        //    if (hasErrorCode == false)
        //        await ChangeStatusAsync(SnappApiEnum.PickApi, marketPlaceOrder, response.response.IsSuccess, response.response?.ErrorMessage);
        //    return response.response;
        //}
        //public async Task<MyResponse<SnappMarketNullResponseDto>?> AcceptOrderAsync(AcceptOrderDto dto, int marketPlaceOrderId)
        //{
        //    var marketPlaceOrder = await _dbContext.MarketPlaceOrders.FirstOrDefaultAsync(x => x.Id == marketPlaceOrderId && x.MarketPlaceTypeId == (int)MarketPlaceTypeEnum.SnappMarket) ?? throw new Exception($" با کد سفارش {marketPlaceOrderId} یافت نشد");
        //    if ((marketPlaceOrder.AcceptApiOn != null || marketPlaceOrder.RejectApiOn != null) && marketPlaceOrder.DocStatusId != (int)DocStatusEnum.Draft || !marketPlaceOrder.IsActive)
        //        throw new Exception("وضعیت سفارش تغییر یافته است");
        //    if (marketPlaceOrder.StatusCode == "51" || marketPlaceOrder.StatusCode == "71" || marketPlaceOrder.StatusCode == "54")
        //    {
        //        await ChangeStatusAsync(SnappApiEnum.RejectApi, marketPlaceOrder, true, null);
        //        throw new Exception(" سفارش قبلا کنسل شده است");
        //    }
        //    if (marketPlaceOrder.StatusCode == "42")
        //    {
        //        await ChangeStatusAsync(SnappApiEnum.AcceptApi, marketPlaceOrder, true, null);
        //        throw new Exception("سفارش قبلا تایید شده است");
        //    }
        //    await ChangeStatusByUser(SnappApiEnum.AcceptApi, marketPlaceOrder);
        //    var token = await GetToken() ?? throw new Exception("توکن دریافت نگردید");
        //    var url = UrlMaker(SnappApiEnum.AcceptApi, marketPlaceOrder.OrderCode);
        //    var json = JsonConvert.SerializeObject(dto);
        //    var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(url, json, token, RequestTypeEnum.Post);
        //    var hasErrorCode = await ErrorCodeCheckAsync(marketPlaceOrder, response.codeErrorMessage);
        //    if (hasErrorCode == false)
        //        await ChangeStatusAsync(SnappApiEnum.AcceptApi, marketPlaceOrder, response.response.IsSuccess, response.response?.ErrorMessage);
        //    return response.response;
        //}
        //public async Task<MyResponse<SnappMarketNullResponseDto>?> RejectOrderAsync(MarketPlaceRejectDto dto)
        //{
        //    var marketPlaceOrder = await _dbContext.MarketPlaceOrders.FirstOrDefaultAsync(x => x.Id == dto.MarketPlaceOrderId && x.OrderCode == dto.OrderCode && x.MarketPlaceTypeId == (int)MarketPlaceTypeEnum.SnappMarket) ?? throw new Exception($" با کد سفارش {dto.MarketPlaceOrderId} یافت نشد");
        //    if (marketPlaceOrder.RejectApiOn != null || marketPlaceOrder.DocStatusId == (int)DocStatusEnum.Canceled || !marketPlaceOrder.IsActive)
        //        throw new Exception(" سفارش قبلا کنسل شده است");
        //    if (marketPlaceOrder.StatusCode == "51" || marketPlaceOrder.StatusCode == "71" || marketPlaceOrder.StatusCode == "54")
        //    {
        //        await ChangeStatusAsync(SnappApiEnum.RejectApi, marketPlaceOrder, true, null);
        //        throw new Exception(" سفارش قبلا کنسل شده است");
        //    }
        //    await ChangeStatusByUser(SnappApiEnum.RejectApi, marketPlaceOrder);
        //    var token = await GetToken() ?? throw new Exception("توکن دریافت نگردید");
        //    var url = UrlMaker(SnappApiEnum.RejectApi, marketPlaceOrder.OrderCode);
        //    var rejectDto = new
        //    {
        //        reasonId = dto.RejectReasonId,
        //        comment = dto.Comment,
        //        nonExistentProducts = dto.NonExistentProducts
        //    };
        //    var json = JsonConvert.SerializeObject(rejectDto);
        //    var response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(url, json, token, RequestTypeEnum.Post);
        //    var hasErrorCode = await ErrorCodeCheckAsync(marketPlaceOrder, response.codeErrorMessage);
        //    if (hasErrorCode == null)
        //    {
        //        json = JsonConvert.SerializeObject(new
        //        {
        //            comment = rejectDto.comment,
        //            reasonId = 119,
        //            nonExistentProducts = rejectDto.nonExistentProducts
        //        });
        //        response = await ConnectToSnappMarketAsync<SnappMarketNullResponseDto>(url, json, token, RequestTypeEnum.Post);
        //        hasErrorCode = await ErrorCodeCheckAsync(marketPlaceOrder, response.codeErrorMessage);
        //    }
        //    if (hasErrorCode == false)
        //        await ChangeStatusAsync(SnappApiEnum.RejectApi, marketPlaceOrder, response.response.IsSuccess, response.response?.ErrorMessage, dto.RejectReasonId);
        //    return response.response;
        //}
        #endregion
    }
}
