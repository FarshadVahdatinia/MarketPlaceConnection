namespace GPOS.Core.Dtos
{
    public class SnappMarketAddOrUpdateProductDto
    {
        public SnappMarketAddOrUpdateProductDto()
        {
            products = new List<SnappMarketProductDto>();
        }

        public string? vendorCode { get; set; }
        public List<SnappMarketProductDto> products { get; set; }
    }
}
