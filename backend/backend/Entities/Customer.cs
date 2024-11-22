using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        [MaxLength(200)]
        public string EmailVerificationToken { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        [MaxLength(100)]
        public string HashPassword { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }

        // Navigation property
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public decimal TotalAmount
        {
            get
            {
                return CartItems.Sum(item => item.Quantity * item.ProductSize.Price);
            }
        }
    }
}
