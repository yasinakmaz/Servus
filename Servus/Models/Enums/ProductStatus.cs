namespace Servus.Models.Enums
{
    public enum ProductStatus
    {
        [Description ("Stok Aktif")]
        Active = 0,
        [Description("Stok Aktif Değil")]
        Inactive = 1,
        [Description("Ürün Stokta Yok")]
        OutOfStock = 2,
    }
}
