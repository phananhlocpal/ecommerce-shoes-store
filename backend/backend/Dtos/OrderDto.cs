using backend.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class OrderReadDto
    {
        public int OrderID { get; set; }
        public DateTime DateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } 
        public List<OrderItemReadDto> OrderItems { get; set; }
    }

    public class OrderCreateDto
    {
        public int CustomerID { get; set; }
        public string? TransactionID { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderUpdateDto
    {
        public string Status { get; set; }
    }
}
