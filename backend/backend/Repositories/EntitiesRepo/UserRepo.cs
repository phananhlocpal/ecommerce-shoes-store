using backend.Entities;

namespace backend.Repositories.EntitiesRepo
{
    public class UserRepo : GenericRepo<User>
    {
        public UserRepo(EcommerceDBContext context) : base(context)
        {
        }
    }
}
