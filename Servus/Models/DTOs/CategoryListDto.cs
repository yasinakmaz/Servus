namespace Servus.Models.DTOs
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
