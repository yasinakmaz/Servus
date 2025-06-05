namespace Servus.Models.Enums
{
    public enum CacheKeys
    {
        [Description("products_all")]
        ProductsAll,

        [Description("products_active")]
        ProductsActive,

        [Description("categories_all")]
        CategoriesAll,

        [Description("categories_active")]
        CategoriesActive,

        [Description("prices_all")]
        PricesAll,

        [Description("prices_active")]
        PricesActive,

        [Description("prices_first_choice")]
        PricesFirstChoice,

        [Description("prices_second_choice")]
        PricesSecondChoice
    }
}
