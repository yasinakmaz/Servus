namespace Servus.Models.DTOs
{
    public class CreatePriceDto
    {
        [Required(ErrorMessage = "Fiyat adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Fiyat adı 2-100 karakter arasında olmalıdır")]
        public string PriceName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false;
        public bool FirstChoice { get; set; } = false;
        public bool SecondChoice { get; set; } = false;

        [Required(ErrorMessage = "Para birimi seçimi zorunludur")]
        public int Currency { get; set; }

        [Required(ErrorMessage = "Fiyat tutarı zorunludur")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat negatif olamaz")]
        public decimal PriceAmount { get; set; }

        [StringLength(100, ErrorMessage = "Oluşturan kişi bilgisi maksimum 100 karakter olabilir")]
        public string? CreatedBy { get; set; }
    }
}
