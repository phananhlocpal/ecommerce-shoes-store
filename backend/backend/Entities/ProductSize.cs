using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Entities
{
    public class ProductSize
    {
        [Key]
        public int ProductSizeID { get; set; }

        [Required]
        public int Size { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Foreign key and relationship
        [Required]
        public int ProductID { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
