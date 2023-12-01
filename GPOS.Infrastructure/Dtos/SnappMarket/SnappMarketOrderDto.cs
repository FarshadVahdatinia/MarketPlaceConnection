using GPOS.Infrastructure.Dtos.SnappMarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Infrastructure.Dtos.SnappMarket
{
    public class SnappMarketOrderDto
    {
        public SnappMarketOrderDto()
        {

            Products = new List<SnappMarketOrderDetailDto>();
        }
        public int Id { get; set; }
        public int MarketPlaceTypeId { get; set; }
        public string? VendorCode { get; set; }
        public string? Code { get; set; }      //  OrderCode For Snapp Version 
        public long? OrderDate { get; set; }
        public DateTime? NewOrderDate { get; set; }
        public int StatusCode { get; set; }
        public string? ExpeditionType { get; set; }
        public string? UserAddressCode { get; set; }//
        public string? UserCode { get; set; }//
        public int VendorMaxPreparationTime { get; set; }//
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? DeliverAddress { get; set; }
        public string? Comment { get; set; }
        public string? Phone { get; set; }
        public long? Amount { get; set; }
        public int? Tax { get; set; }
        public int? DeliveryPrice { get; set; }
        public int? PackingPrice { get; set; }
        public int? DeliveryTime { get; set; }
        public int? PreparationTime { get; set; }
        public double? Vat { get; set; }
        public int? TaxCoeff { get; set; }
        public string? DiscountType { get; set; }
        public int? DiscountValue { get; set; }
        public string? OrderPaymentTypeCode { get; set; }
        public string? OrderPayment { get; set; }
        public DateTime CreatedOn { get; set; }
        public int DocStatusId { get; set; }
        public string? AckApiError { get; set; }
        public DateTime? AckApiErrorOn { get; set; }
        public DateTime? AckApiOn { get; set; }
        public int? PickBy { get; set; }
        public DateTime? PickOn { get; set; }
        public string? PickApiError { get; set; }
        public DateTime? PickApiErrorOn { get; set; }
        public DateTime? PickApiOn { get; set; }
        public int? AcceptBy { get; set; }
        public DateTime? AcceptOn { get; set; }
        public string? AcceptApiError { get; set; }
        public DateTime? AcceptApiErrorOn { get; set; }
        public DateTime? AcceptApiOn { get; set; }
        public int? RejectBy { get; set; }
        public DateTime? RejectOn { get; set; }
        public string? RejectApiError { get; set; }
        public DateTime? RejectApiErrorOn { get; set; }
        public DateTime? RejectApiOn { get; set; }
        public string? RejectReason { get; set; }
        public string? ActComment { get; set; }
        //public string Products { get; set; }
        public List<SnappMarketOrderDetailDto> Products { get; set; }
    }
}
