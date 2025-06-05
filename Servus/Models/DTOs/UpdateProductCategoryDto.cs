namespace Servus.Models.DTOs
{
    public class UpdateProductCategoryDto
    {
        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Kategori adı 2-100 karakter arasında olmalıdır")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama maksimum 500 karakter olabilir")]
        public string? Description { get; set; }

        [Range(1, 999, ErrorMessage = "Görüntüleme sırası 1-999 arasında olmalıdır")]
        public int DisplayOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        [StringLength(100, ErrorMessage = "Güncelleyen kişi bilgisi maksimum 100 karakter olabilir")]
        public string? UpdatedBy { get; set; }
    }
}
