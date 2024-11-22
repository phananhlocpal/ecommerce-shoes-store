using backend.Dtos;
using backend.Entities;
using backend.Models;

namespace backend.Repositories.AuthRepo
{
    public interface IAuthRepo
    {
        Task<CustomerReadDto> ValidateCustomerCredentialsAsync(string email, string password);
        Task<Customer> RegisterCustomerAsync(CustomerSignUpVM customerSignUpVM);
        Task<User> ValidateUserCredentialsAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<Customer> GetCustomerByEmailAsync(string email);
    }
}
