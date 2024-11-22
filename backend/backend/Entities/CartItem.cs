using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("ProductSize")]
        public int? ProductSizeID { get; set; }

        public ProductSize ProductSize { get; set; }

        public int Quantity { get; set; }
    }
}
