namespace Servus.Models.Entities
{
    [Table("Prices")]
    [Index(nameof(IsActive), Name = "IX_Prices_IsActive")]
    [Index(nameof(IsDefault), Name = "IX_Prices_IsDefault")]
    [Index(nameof(Currency), Name = "IX_Prices_Currency")]
    public class Price
    {
        [Key]
        [Column("IND")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IND { get; set; }

        [Required]
        [StringLength(100)]
        [Column("PriceName")]
        public string PriceName { get; set; } = string.Empty;

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("IsDefault")]
        public bool IsDefault { get; set; } = false;

        [Column("FirstChoice")]
        public bool FirstChoice { get; set; } = false;

        [Column("SecondChoice")]
        public bool SecondChoice { get; set; } = false;

        [Column("Currency")]
        public int Currency { get; set; }

        [Column("Price", TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat negatif olamaz")]
        public decimal PriceAmount { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<ProductFirstChoice> FirstChoicePrices { get; set; } = new List<ProductFirstChoice>();
        public virtual ICollection<ProductSecondChoice> SecondChoicePrices { get; set; } = new List<ProductSecondChoice>();
    }
}
