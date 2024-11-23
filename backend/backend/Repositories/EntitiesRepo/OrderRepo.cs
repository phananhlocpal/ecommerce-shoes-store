using backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace backend.Repositories.EntitiesRepo
{
    public class OrderRepo : GenericRepo<Order>
    {
        public OrderRepo(EcommerceDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetByConditionAsync(Expression<Func<Order, bool>> expression)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductSize)
                        .ThenInclude(ps => ps.Product)
                .Where(expression);
        }
    }
}
