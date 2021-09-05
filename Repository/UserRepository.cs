using Repository.Extensions;
using CoreLibrary;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = Username bị trùng, null: thêm thành công
        public User CreateUser(User user)
        {
            //Mặc định
            user.HinhAnh = "cat-lie.png";
            Create(user);
            return user;
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: Username bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateUser(User user)
        {
            Update(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa user thành công" };
        }

        public ResponseDetails UpdateUserRefreshToken(User user, string newRefreshToken, DateTime? newRefreshTokenExipredTime)
        {
            user.RefreshToken = newRefreshToken;
            if (newRefreshTokenExipredTime != null) user.RefreshTokenExpiryTime = (DateTime)newRefreshTokenExipredTime;
            Update(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Cập nhật refresh token thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteUser(User user)
        {
            user.TinhTrang = true;
            Update(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa user thành công" };
        }

        //Lấy danh sách các user không bị xóa
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await FindAll()
                .Where(user => !user.TinhTrang)
                .OrderBy(user => user.UserID)
                .ToListAsync();
        }

        //Dùng cho lấy user để get/update/delete
        public async Task<User> GetUserByIDAsync(string userID)
        {
            return await FindByCondition(user => user.UserID.ToString() == userID && !user.TinhTrang)
                    .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByApplicationUserIDAsync(string applicationUserID)
        {
            return await FindByCondition(user => user.ApplicationUserID == applicationUserID && !user.TinhTrang)
                    .FirstOrDefaultAsync();
        }

        //Dùng cho khi đã login rồi mà muốn lấy thông tin của user
        public async Task<User> GetUserByUserIDDetailAsync(string userID)
        {
            return await FindByCondition(user => user.UserID.ToString() == userID && !user.TinhTrang)
                .FirstOrDefaultAsync();
        }
    }
}
