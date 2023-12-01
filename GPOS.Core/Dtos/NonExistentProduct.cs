namespace GPOS.Core.Dtos
{
    public class NonExistentProduct
    {
        public NonExistentProduct()
        {
            suggestedProductBarcodes = new List<string>();
        }
        public string? barcode { get; set; }
        public List<string>? suggestedProductBarcodes { get; set; }
    }
}
