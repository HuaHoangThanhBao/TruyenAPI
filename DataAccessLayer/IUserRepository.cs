using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByEmailAsync(string userID);
        Task<User> GetUserByIDAsync(string userID);
        Task<User> GetUserByUserIDDetailAsync(string userID);
        ResponseDetails CreateUser(User user);
        ResponseDetails UpdateUser(User user);
        ResponseDetails DeleteUser(User user);
    }
}
