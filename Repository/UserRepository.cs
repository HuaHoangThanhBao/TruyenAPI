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
        //KQ: !null = Username bị trùng, null: thêm thành công
        public ResponseDetails CreateUser(User user)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(user.FirstName))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên không được chứa ký tự đặc biệt",
                    Value = user.FirstName.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(user.LastName))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Họ không được chứa ký tự đặc biệt",
                    Value = user.LastName.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [Email]*/
            if (FindByCondition(t => t.Email.Equals(user.Email)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Email bị trùng",
                    Value = user.Email.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [Username]*/
            if (FindByCondition(t => t.FirstName.Equals(user.FirstName) && t.LastName.Equals(user.LastName)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Họ tên bị trùng",
                    Value = user.Username.ToString()
                };
            }
            /*End*/

            if (user.FirstName == "" || user.FirstName == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên không được để trống",
                    Value = user.Username.ToString()
                };
            }

            if (user.LastName == "" || user.LastName == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Họ được để trống",
                    Value = user.Username.ToString()
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
            user.Username = user.LastName + user.FirstName;
            user.Password = passHash;

            Create(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: Username bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateUser(User user)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(user.FirstName))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên không được chứa ký tự đặc biệt",
                    Value = user.FirstName.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(user.LastName))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Họ không được chứa ký tự đặc biệt",
                    Value = user.LastName.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [Email]*/
            if (FindByCondition(t => t.Email.Equals(user.Email) && t.UserID != user.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Email bị trùng",
                    Value = user.Email.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [Username]*/
            if (FindByCondition(t => t.FirstName.Equals(user.FirstName) && t.LastName.Equals(user.LastName) && t.UserID != user.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Họ tên bị trùng",
                    Value = user.Username.ToString()
                };
            }
            /*End*/

            if (user.FirstName == "" || user.FirstName == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên không được để trống",
                    Value = user.Username.ToString()
                };
            }

            if (user.LastName == "" || user.LastName == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Họ được để trống",
                    Value = user.Username.ToString()
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

        //Lấy danh sách các user không bị xóa
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await FindAll()
                .Where(ow => !ow.TinhTrang)
                .OrderBy(ow => ow.Username)
                .ToListAsync();
        }

        public async Task<User> GetUserByNameAsync(string Username)
        {
            return await FindByCondition(user => user.Username.Equals(Username))
                    .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByDetailAsync(string Username)
        {
            return await FindByCondition(user => user.Username.Equals(Username))
                .Include(a => a.TheoDois)
                    .ThenInclude(b => b.Truyen)
                .FirstOrDefaultAsync();
        }
    }
}
