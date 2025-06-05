namespace Servus.Models.DTOs
{
    public class ProductListDto
    {
        public int IND { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public ProductStatus Status { get; set; }
        public string StatusDescription { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string PriceName { get; set; } = string.Empty;
        public decimal PriceAmount { get; set; }
        public bool IsImageAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
