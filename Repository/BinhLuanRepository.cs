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
    public class BinhLuanRepository : RepositoryBase<BinhLuan>, IBinhLuanRepository
    {
        public BinhLuanRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenBinhLuan bị trùng, null: thêm thành công
        public ResponseDetails CreateBinhLuan(BinhLuan binhLuan)
        {
            Create(binhLuan);
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenBinhLuan bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateBinhLuan(BinhLuan binhLuan)
        {
            Update(binhLuan);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa bình luận thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteBinhLuan(BinhLuan binhLuan)
        {
            Delete(binhLuan);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa bình luận thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<BinhLuan>> GetAllBinhLuansAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.BinhLuanID)
                .ToListAsync();
        }

        public async Task<BinhLuan> GetBinhLuanByIdAsync(int binhLuanId)
        {
            return await FindByCondition(binhLuan => binhLuan.BinhLuanID.Equals(binhLuanId))
                    .FirstOrDefaultAsync();
        }

        public async Task<BinhLuan> GetBinhLuanByDetailAsync(int binhLuanId)
        {
            return await FindByCondition(BinhLuan => BinhLuan.BinhLuanID.Equals(binhLuanId))
                .Include(ac => ac.Truyen)
                .Include(ac => ac.User)
                .FirstOrDefaultAsync();
        }
    }
}
