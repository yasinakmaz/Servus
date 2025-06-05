namespace Servus.Models.Enums
{
    public enum LogLevel
    {
        [Description("Bilgi")]
        Information = 0,

        [Description("Uyarı")]
        Warning = 1,

        [Description("Hata")]
        Error = 2,

        [Description("Debug")]
        Debug = 3,

        [Description("Kritik")]
        Critical = 4
    }
}
