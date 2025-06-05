namespace Servus.Models.Entities
{
    [Table("ProductFirstChoices")]
    [Index(nameof(ProductId), Name = "IX_ProductFirstChoices_ProductId")]
    [Index(nameof(ProductPriceId), Name = "IX_ProductFirstChoices_ProductPriceId")]
    [Index(nameof(ChoiceName), Name = "IX_ProductFirstChoices_ChoiceName")]
    public class ProductFirstChoice
    {
        [Key]
        [Column("IND")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IND { get; set; }

        // Foreign Key Properties
        [Column("ProductIND")]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Column("ProductPriceIND")]
        [ForeignKey(nameof(ProductPrice))]
        public int ProductPriceId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("ChoiceName")]
        public string ChoiceName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [Range(1, 999)]
        public int DisplayOrder { get; set; } = 1;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Price ProductPrice { get; set; } = null!;
    }
}
