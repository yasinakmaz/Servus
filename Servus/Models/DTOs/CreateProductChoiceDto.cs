namespace Servus.Models.DTOs
{
    public class CreateProductChoiceDto
    {
        [Required(ErrorMessage = "Seçenek adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Seçenek adı 2-100 karakter arasında olmalıdır")]
        public string ChoiceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat referansı zorunludur")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir fiyat referansı seçiniz")]
        public int ProductPriceId { get; set; }

        [Range(1, 999, ErrorMessage = "Görüntüleme sırası 1-999 arasında olmalıdır")]
        public int DisplayOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        [StringLength(100, ErrorMessage = "Oluşturan kişi bilgisi maksimum 100 karakter olabilir")]
        public string? CreatedBy { get; set; }
    }
}
