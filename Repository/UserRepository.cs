using Repository.Extensions;
using CoreLibrary;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
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
            user.Username = (user.Username == "" || user.Username == null) ? user.FirstName + user.LastName : user.Username;

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
            if (FindByCondition(t => t.Username.Equals(user.Username)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Username bị trùng",
                    Value = user.Username.ToString()
                };
            }
            /*End*/

            string passHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passHash;

            Create(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: Username bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateUser(User user)
        {
            user.Username = (user.Username == "" || user.Username == null) ? user.FirstName + user.LastName : user.Username;

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
            if (FindByCondition(t => t.Username.Equals(user.Username) && t.UserID != user.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Username bị trùng",
                    Value = user.Username.ToString()
                };
            }
            /*End*/

            string passHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passHash;

            Update(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa user thành công" };
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
                .OrderBy(user => user.Username)
                .ToListAsync();
        }

        //Dùng cho khi xác thực login và lấy ra mã GUID để lưu lại phía client
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await FindByCondition(user => user.Email.Equals(email))
                    .FirstOrDefaultAsync();
        }

        //Dùng cho lấy user để get/update/delete
        public async Task<User> GetUserByIDAsync(string userID)
        {
            return await FindByCondition(user => user.UserID.ToString() == userID && !user.TinhTrang)
                    .FirstOrDefaultAsync();
        }

        //Dùng cho khi đã login rồi mà muốn lấy thông tin của user
        public async Task<User> GetUserByUserIDDetailAsync(string userID)
        {
            return await FindByCondition(user => user.UserID.ToString() == userID && !user.TinhTrang)
                .Include(user => user.BinhLuans)
                    .ThenInclude(b => b.Chuong)
                        .ThenInclude(c => c.Truyen)
                .Include(user => user.TheoDois)
                    .ThenInclude(b => b.Truyen)
                .FirstOrDefaultAsync();
        }
    }
}
