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
            Create(user);
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenUser bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateUser(User user)
        {
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
                .Include(ac => ac.TheoDois)
                .FirstOrDefaultAsync();
        }
    }
}
