using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationApp_4_5_2.Data.Entities;

namespace RegistrationApp_4_5_2.Data.Abstraction
{
    public interface ISqliteDataAccess
    {
        Task<IEnumerable<User>> LoadUsersAsync(string sortingProperty);
        Task SaveUserAsync(User newUser);
        Task<bool> IsFullNameExistsAsync(string fullName);
    }
}