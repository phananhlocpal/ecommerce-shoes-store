using backend.Entities;

namespace backend.Repositories.EntitiesRepo
{
    public class ProductSizeRepo : GenericRepo<ProductSize>
    {
        public ProductSizeRepo(EcommerceDBContext context) : base(context)
        {
        }
    }
}
