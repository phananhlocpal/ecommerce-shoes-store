using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo
{
    public class ProductRepo : GenericRepo<Product>
    {
        public ProductRepo(EcommerceDBContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                    .Include(p => p.ProductSizes)
                    .Include(p => p.Category)
                    .ToListAsync();
        }
    }
}
