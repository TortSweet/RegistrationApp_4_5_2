using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ninject;
using RegistrationApp_4_5_2.Data;
using RegistrationApp_4_5_2.Data.Abstraction;
using RegistrationApp_4_5_2.Data.Entities;
using RegistrationApp_4_5_2.Services.Abstraction;

namespace RegistrationApp_4_5_2.Services
{
    public class UserService : IUserService
    {
        private readonly ISqliteDataAccess _dataAccess;

        public UserService()
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ISqliteDataAccess>().To<SqliteDataAccess>();
            _dataAccess = ninjectKernel.Get<ISqliteDataAccess>();
        }
        
        /*public UserService(ISqliteDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess), "Access to Db must exist");
        }*/

        public Task<IEnumerable<User>> GetUsersListAsync(string sortingProperty)
        {
           return _dataAccess.LoadUsersAsync(sortingProperty);
        }

        public Task SaveUserAsync(User newUser)
        {
            if (newUser == null)
            {
                throw new ArgumentNullException(nameof(newUser), "New user must exist");
            }

            return _dataAccess.SaveUserAsync(newUser);
        }

        public Task<bool> IsUserExistAsync(string fullName)
        {
            return _dataAccess.IsFullNameExistsAsync(fullName);
        }
    }
}