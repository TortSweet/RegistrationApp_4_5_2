using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationApp_4_5_2.Data.Entities;

namespace RegistrationApp_4_5_2.Services.Abstraction
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersListAsync(string sortingProperty);
        Task SaveUserAsync(User newUser);
        Task<bool> IsUserExistAsync(string fullName);
    }
}