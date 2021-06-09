using Repository.Extensions;
using CoreLibrary;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenUser bị trùng, null: thêm thành công
        public ResponseDetails CreateUser(User user)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(user.TenUser))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Không được chứa ký tự đặc biệt",
                    Value = user.TenUser.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [TenUser]*/
            if (FindByCondition(t => t.TenUser.Equals(user.TenUser)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên user bị trùng",
                    Value = user.TenUser.ToString()
                };
            }
            /*End*/

            if (user.TenUser == "" || user.TenUser == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên user không được để trống",
                    Value = user.TenUser.ToString()
                };
            }

            if (user.Password == "" || user.Password == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Password không được để trống",
                    Value = user.Password.ToString()
                };
            }

            string passHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passHash;

            Create(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenUser bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateUser(User user)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(user.TenUser))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Không được chứa ký tự đặc biệt",
                    Value = user.TenUser.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [TenUser]*/
            if (FindByCondition(t => t.TenUser.Equals(user.TenUser)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên user bị trùng",
                    Value = user.TenUser.ToString()
                };
            }
            /*End*/

            if (user.TenUser == "" || user.TenUser == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên user không được để trống",
                    Value = user.TenUser.ToString()
                };
            }

            if (user.Password == "" || user.Password == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Password không được để trống",
                    Value = user.Password.ToString()
                };
            }

            string passHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passHash;

            Update(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa user thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteUser(User User)
        {
            User.TinhTrang = true;
            Update(User);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa user thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await FindAll()
                .Where(ow => !ow.TinhTrang)
                .OrderBy(ow => ow.TenUser)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await FindByCondition(user => user.UserID.Equals(userId))
                    .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByDetailAsync(Guid userId)
        {
            return await FindByCondition(user => user.UserID.Equals(userId))
                .Include(a => a.TheoDois)
                    .ThenInclude(b => b.Truyen)
                .FirstOrDefaultAsync();
        }
    }
}
