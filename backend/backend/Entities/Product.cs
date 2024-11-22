using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Brand { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Url]
        public string ImageURL { get; set; }

        // Foreign key and relationship
        [Required]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        // Navigation property
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
