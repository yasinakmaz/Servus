namespace Servus.Models.Enums
{
    public enum FileOperationResult
    {
        [Description("Başarılı")]
        Success = 0,

        [Description("Dosya bulunamadı")]
        FileNotFound = 1,

        [Description("Geçersiz format")]
        InvalidFormat = 2,

        [Description("Boyut çok büyük")]
        FileTooLarge = 3,

        [Description("Okuma hatası")]
        ReadError = 4,

        [Description("Yazma hatası")]
        WriteError = 5
    }
}
