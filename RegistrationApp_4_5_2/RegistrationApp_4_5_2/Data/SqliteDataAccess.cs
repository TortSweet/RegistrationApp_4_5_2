using System;
using Dapper;
using RegistrationApp_4_5_2.Data.Abstraction;
using RegistrationApp_4_5_2.Data.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationApp_4_5_2.Data
{
    public class SqliteDataAccess : ISqliteDataAccess
    {
        private string _sqlLiteConnection;
        
        public SqliteDataAccess()
        {
            _sqlLiteConnection = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }
        public async Task<IEnumerable<User>> LoadUsersAsync(string sortingProperty)
        {
            using (var connection = new SQLiteConnection(_sqlLiteConnection))
            {
                await connection.OpenAsync();
                // If a database connection error occurs, you should use the absolute path to connect to the database

                var outPut = (await connection.QueryAsync<User>("select * from Users",
                    new DynamicParameters())).AsQueryable();

                outPut = SortUsers(outPut, sortingProperty);

                connection.Close();
                return outPut.ToArray(); 
            };
        }

        public async Task SaveUserAsync(User newUser)
        {
            using (var connection = new SQLiteConnection(_sqlLiteConnection))
            {
                await connection.OpenAsync();
                var savedUser = await connection.ExecuteAsync(
                    "insert into Users (FullName, Age, City, Email, PhoneNumber) values (@FullName, @Age, @City, @Email, @PhoneNumber)",
                    newUser);
                connection.Close(); 
            };
        }

        public async Task<bool> IsFullNameExistsAsync(string fullName)
        {
            using (var connection = new SQLiteConnection(_sqlLiteConnection))
            {
                await connection.OpenAsync();
                var outPut = await connection.QueryFirstOrDefaultAsync<User>("select * from Users where FullName == @FullName", new {FullName = fullName});
                connection.Close();
                return outPut != null; 
            };
        }

        private static IQueryable<User> SortUsers(IQueryable<User> userList, string property)
        {
            switch (property)
            {
                    case "FullName":
                        userList = userList.OrderBy(item => item.FullName);
                        break;
                    case "Id":
                        userList = userList.OrderBy(item => item.Id);
                        break;
                    case "Age":
                        userList = userList.OrderBy(item => item.Age);
                        break;
                    case "City":
                        userList = userList.OrderBy(item => item.City);
                        break;
                    case "Email":
                        userList = userList.OrderBy(item => item.Email);
                        break;
                    case "PhoneNumber":
                        userList = userList.OrderBy(item => item.PhoneNumber);
                        break;
            }
            return userList;
        }
    }
}