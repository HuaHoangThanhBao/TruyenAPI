using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIDAsync(string userID);
        Task<User> GetUserByApplicationUserIDAsync(string applicationUserID);
        Task<User> GetUserByUserIDDetailAsync(string userID);
        User CreateUser(User user);
        ResponseDetails UpdateUser(User user);
        ResponseDetails UpdateUserRefreshToken(User user, string newRefreshToken, DateTime? newRefreshTokenExipredTime);
        ResponseDetails DeleteUser(User user);
    }
}
