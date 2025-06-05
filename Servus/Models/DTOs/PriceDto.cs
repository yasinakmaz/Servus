namespace Servus.Models.DTOs
{
    public class PriceDto
    {
        public int IND { get; set; }
        public string PriceName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool FirstChoice { get; set; }
        public bool SecondChoice { get; set; }
        public int Currency { get; set; }
        public decimal PriceAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
