using backend.Dtos;

namespace backend.Models
{
    public class VnPaymentRequestModel
    {
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public OrderCreateDto Order { get; set; }
    }
}
