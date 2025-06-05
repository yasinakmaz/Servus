namespace Servus.Models.Enums
{
    public enum ValidationResult
    {
        [Description("Başarılı")]
        Success = 0,

        [Description("Genel hata")]
        Error = 1,

        [Description("Veri bulunamadı")]
        NotFound = 2,

        [Description("Geçersiz veri")]
        InvalidData = 3,

        [Description("Yetki yok")]
        Unauthorized = 4,

        [Description("Duplikasyon hatası")]
        Duplicate = 5
    }
}
