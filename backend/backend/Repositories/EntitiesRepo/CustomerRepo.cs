using backend.Entities;

namespace backend.Repositories.EntitiesRepo
{
    public class CustomerRepo : GenericRepo<Customer>
    {
        public CustomerRepo(EcommerceDBContext context) : base(context)
        {

        }
    }
}
