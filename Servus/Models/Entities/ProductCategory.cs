namespace Servus.Models.Entities
{
    [Table("ProductCategories")]
    [Index(nameof(CategoryName), Name = "IX_ProductCategories_CategoryName", IsUnique = true)]
    [Index(nameof(IsActive), Name = "IX_ProductCategories_IsActive")]
    public class ProductCategory
    {
        [Key]
        [Column("IND")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("CategoryName")]
        public string CategoryName { get; set; } = string.Empty;

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 999)]
        public int DisplayOrder { get; set; } = 1;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
