using backend.Entities;

namespace backend.Repositories.EntitiesRepo
{
    public class OrderItemRepo : GenericRepo<OrderItem>
    {
        public OrderItemRepo(EcommerceDBContext context) : base(context)
        {
        }
    }
}