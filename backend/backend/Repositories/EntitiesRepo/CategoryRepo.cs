using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo
{
    public class CategoryRepo : GenericRepo<Category>
    {
        public CategoryRepo(EcommerceDBContext context) : base(context)
        {

        }
    }
}
