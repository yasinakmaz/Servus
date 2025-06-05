namespace Servus.Models.DTOs
{
    public class ProductDto
    {
        public int IND { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public ProductStatus Status { get; set; }
        public bool IsImageAvailable { get; set; }
        public bool IsImageType { get; set; }
        public string? ImageUrl { get; set; }
        public byte[]? Image { get; set; }
        public int DisplayOrder { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int PriceId { get; set; }
        public string PriceName { get; set; } = string.Empty;
        public decimal PriceAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public List<ProductFirstChoiceDto> FirstChoices { get; set; } = new();
        public List<ProductSecondChoiceDto> SecondChoices { get; set; } = new();
    }
}
