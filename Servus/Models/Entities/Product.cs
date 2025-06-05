namespace Servus.Models.Entities
{
    [Table("Products")]
    [Index(nameof(ProductCode), Name = "IX_Products_ProductCode", IsUnique = true)]
    [Index(nameof(ProductName), Name = "IX_Products_ProductName")]
    [Index(nameof(Status), Name = "IX_Products_IsActive")]
    [Index(nameof(CategoryId), Name = "IX_Products_CategoryId")]
    [Index(nameof(PriceId), Name = "IX_Products_PriceId")]
    public class Product
    {
        [Key]
        [Column("IND")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IND { get; set; }

        [Required]
        public ProductStatus Status { get; set; } = ProductStatus.Active;

        [Required]
        [StringLength(200)]
        [Column("ProductName")]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("ProductCode")]
        public string ProductCode { get; set; } = string.Empty;

        [Column("IsImageAvailable")]
        public bool IsImageAvailable { get; set; } = false;

        [Column("IsImageType")]
        public bool IsImageType { get; set; } = false;

        [StringLength(500)]
        [Column("ImageUrl")]
        public string? ImageUrl { get; set; }

        [Column("Image")]
        public byte[]? Image { get; set; }

        [Range(1, 999)]
        public int DisplayOrder { get; set; } = 1;

        [Column("PriceInd")]
        [ForeignKey(nameof(Price))]
        public int PriceId { get; set; }

        [Column("CategoryInd")]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        public virtual Price Price { get; set; } = null!;
        public virtual ProductCategory Category { get; set; } = null!;
        public virtual ICollection<ProductFirstChoice> FirstChoices { get; set; } = new List<ProductFirstChoice>();
        public virtual ICollection<ProductSecondChoice> SecondChoices { get; set; } = new List<ProductSecondChoice>();
    }
}
