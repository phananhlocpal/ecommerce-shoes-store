using backend.Entities;

namespace backend.Repositories.EntitiesRepo
{
    public class OrderRepo : GenericRepo<Order>
    {
        public OrderRepo(EcommerceDBContext context) : base(context)
        {
        }
    }
}
