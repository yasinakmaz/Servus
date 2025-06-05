namespace Servus.Models.Enums
{
    public enum ServiceOperationResult
    {
        [Description("Başarılı")]
        Success = 0,

        [Description("Başarısız")]
        Failed = 1,

        [Description("Kısmi başarı")]
        PartialSuccess = 2,

        [Description("İptal edildi")]
        Cancelled = 3,

        [Description("Zaman aşımı")]
        Timeout = 4
    }
}
