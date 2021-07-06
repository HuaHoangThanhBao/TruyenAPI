using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByNameAsync(string tenUser);
        Task<User> GetUserByDetailAsync(string tenUser);
        ResponseDetails CreateUser(User user);
        ResponseDetails UpdateUser(User user);
        ResponseDetails DeleteUser(User user);
    }
}
