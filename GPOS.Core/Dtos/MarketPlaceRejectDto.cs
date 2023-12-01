namespace GPOS.Core.Dtos
{
    public class MarketPlaceRejectDto
    {
        public MarketPlaceRejectDto()
        {
            NonExistentProducts = new List<NonExistentProduct>();
        }
        public int MarketPlaceOrderId { get; set; }
        public string? OrderCode { get; set; }
        public int RejectReasonId { get; set; }
        public string? Comment { get; set; }
        public List<NonExistentProduct> NonExistentProducts { get; set; }
    }
}
