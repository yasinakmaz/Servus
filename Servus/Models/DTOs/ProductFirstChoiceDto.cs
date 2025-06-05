namespace Servus.Models.DTOs
{
    public class ProductFirstChoiceDto
    {
        public int IND { get; set; }
        public int ProductId { get; set; }
        public int ProductPriceId { get; set; }
        public string ChoiceName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string PriceName { get; set; } = string.Empty;
        public decimal PriceAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
