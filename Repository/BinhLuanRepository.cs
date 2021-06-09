using CoreLibrary;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class BinhLuanRepository : RepositoryBase<BinhLuan>, IBinhLuanRepository
    {
        private RepositoryContext _context;
        public BinhLuanRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenBinhLuan bị trùng, null: thêm thành công
        public ResponseDetails CreateBinhLuan(BinhLuan binhLuan)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(binhLuan.NoiDung))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Không được chứa ký tự đặc biệt",
                    Value = binhLuan.NoiDung.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [ID]*/
            var userRepo = new UserRepository(_context);
            var chuongRepo = new ChuongRepository(_context);
            if(!userRepo.FindByCondition(t => t.UserID == binhLuan.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID User không tồn tại",
                    Value = binhLuan.UserID.ToString()
                };
            }

            if (!chuongRepo.FindByCondition(t => t.ChuongID == binhLuan.ChuongID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID Chương không tồn tại",
                    Value = binhLuan.ChuongID.ToString()
                };
            }
            /*End*/

            if(binhLuan.NoiDung == "" || binhLuan.NoiDung == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Nội dung chương không được để trống",
                    Value = binhLuan.NoiDung.ToString()
                };
            }

            if (binhLuan.NgayBL == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Ngày bình luận không được để trống",
                    Value = binhLuan.NgayBL.ToString()
                };
            }

            Create(binhLuan);
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenBinhLuan bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateBinhLuan(BinhLuan binhLuan)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(binhLuan.NoiDung))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Không được chứa ký tự đặc biệt",
                    Value = binhLuan.NoiDung.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [ID]*/
            var userRepo = new UserRepository(_context);
            var chuongRepo = new ChuongRepository(_context);
            if (!userRepo.FindByCondition(t => t.UserID == binhLuan.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID User không tồn tại",
                    Value = binhLuan.UserID.ToString()
                };
            }

            if (!chuongRepo.FindByCondition(t => t.ChuongID == binhLuan.ChuongID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID Chương không tồn tại",
                    Value = binhLuan.ChuongID.ToString()
                };
            }
            /*End*/

            if (binhLuan.NoiDung == "" || binhLuan.NoiDung == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Nội dung chương không được để trống",
                    Value = binhLuan.NoiDung.ToString()
                };
            }

            if (binhLuan.NgayBL == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Ngày bình luận không được để trống",
                    Value = binhLuan.BinhLuanID.ToString()
                };
            }

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
                .Include(ac => ac.Chuong)
                    .ThenInclude(ac => ac.Truyen)
                .Include(m => m.User)
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
                .Include(ac => ac.Chuong)
                .Include(ac => ac.User)
                .FirstOrDefaultAsync();
        }
    }
}
