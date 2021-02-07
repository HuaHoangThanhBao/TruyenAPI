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
    public class ChuongRepository : RepositoryBase<Chuong>, IChuongRepository
    {
        private RepositoryContext _context;
        public ChuongRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenChuong bị trùng, null: thêm thành công
        public ResponseDetails CreateChuong(IEnumerable<Chuong> chuongs)
        {
            foreach (var chuong in chuongs)
            {
                if (FindByCondition(t => t.TenChuong.Equals(chuong.TenChuong)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên chương bị trùng",
                        Value = chuong.TenChuong
                    };
                }


                var truyenRepo = new TruyenRepository(_context);
                if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(chuong.TruyenID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "ID truyện không tồn tại",
                        Value = chuong.TruyenID.ToString()
                    };
                }

                Create(chuong);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenChuong bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateChuong(Chuong chuong)
        {
            if (FindByCondition(t => t.TenChuong.Equals(chuong.TenChuong)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên chương bị trùng"
                };
            }

            var truyenRepo = new TruyenRepository(_context);
            if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(chuong.TruyenID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID truyện không tồn tại",
                    Value = chuong.TruyenID.ToString()
                };
            }

            Update(chuong);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa chương thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteChuong(Chuong chuong)
        {
            Delete(chuong);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa chương thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<Chuong>> GetAllChuongsAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.TenChuong)
                .ToListAsync();
        }

        public async Task<Chuong> GetChuongByIdAsync(int chuongId)
        {
            return await FindByCondition(Chuong => Chuong.ChuongID.Equals(chuongId))
                    .FirstOrDefaultAsync();
        }

        public async Task<Chuong> GetChuongByDetailAsync(int chuongId)
        {
            return await FindByCondition(chuong => chuong.ChuongID.Equals(chuongId))
                .Include(ac => ac.Truyen)
                .FirstOrDefaultAsync();
        }
    }
}
