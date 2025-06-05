namespace Servus.Models.Interfaces
{
    public interface IImageService
    {
        Task<byte[]?> ConvertImageToByteArrayAsync(string imageUrl);
        Task<bool> ValidateImageAsync(byte[] imageData);
        Task<bool> ValidateImageUrlAsync(string imageUrl);
        string GetImageContentType(byte[] imageData);
        Task<byte[]?> ResizeImageAsync(byte[] imageData, int maxWidth, int maxHeight);
    }
}
