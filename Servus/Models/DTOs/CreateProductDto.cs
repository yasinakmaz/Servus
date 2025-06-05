namespace Servus.Models.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Ürün adı 2-200 karakter arasında olmalıdır")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ürün kodu zorunludur")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ürün kodu 2-50 karakter arasında olmalıdır")]
        public string ProductCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçiniz")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Fiyat seçimi zorunludur")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir fiyat seçiniz")]
        public int PriceId { get; set; }

        [Range(1, 999, ErrorMessage = "Görüntüleme sırası 1-999 arasında olmalıdır")]
        public int DisplayOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;
        public ImageDto? Image { get; set; }

        public List<CreateProductChoiceDto> FirstChoices { get; set; } = new();
        public List<CreateProductChoiceDto> SecondChoices { get; set; } = new();

        [StringLength(100, ErrorMessage = "Oluşturan kişi bilgisi maksimum 100 karakter olabilir")]
        public string? CreatedBy { get; set; }
    }
}
