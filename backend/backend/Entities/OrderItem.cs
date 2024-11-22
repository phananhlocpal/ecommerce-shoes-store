using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Entities
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }
        [Required]
        public int Quantity { get; set; }

        [Required]
        public int OrderID { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }

        [Required]
        public int ProductSizeID { get; set; }
        [JsonIgnore]
        public ProductSize ProductSize { get; set; }
    }
}
