namespace Servus.Models.DTOs
{
    public class PriceListDto
    {
        public int IND { get; set; }
        public string PriceName { get; set; } = string.Empty;
        public decimal PriceAmount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
