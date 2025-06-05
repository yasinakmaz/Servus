namespace Servus.Models.DTOs
{
    public class ImageDto
    {
        public ImageType ImageType { get; set; } = ImageType.Url;

        [StringLength(500, ErrorMessage = "Resim URL'i maksimum 500 karakter olabilir")]
        [Url(ErrorMessage = "Geçerli bir URL formatı giriniz")]
        public string? ImageUrl { get; set; }

        [MaxLength(5 * 1024 * 1024, ErrorMessage = "Resim boyutu maksimum 5MB olabilir")]
        public byte[]? ImageData { get; set; }
    }
}
