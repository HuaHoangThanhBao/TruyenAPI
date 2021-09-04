using CoreLibrary;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
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
            /*Kiểm tra xem chuỗi json nhập vào có bị trùng tên chương không*/
            foreach (var dup in chuongs.GroupBy(p => p.TenChuong))
            {
                if (dup.Count() - 1 > 0)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Chuỗi json nhập vào bị trùng tên chương",
                        Value = dup.Key.ToString()
                    };
                }
            }
            /*End*/

            foreach (var chuong in chuongs)
            {
                /*Bắt lỗi [ID]*/
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
                /*End*/

                /*Bắt lỗi [Tên chương]*/
                if (FindByCondition(t => t.TenChuong.Equals(chuong.TenChuong) && t.TruyenID.Equals(chuong.TruyenID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên chương bị trùng",
                        Value = chuong.TenChuong
                    };
                }
                /*End*/

                chuong.ThoiGianCapNhat = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                Random r = new Random();
                chuong.LuotXem = r.Next(5000, 15000);

                //Tạo dữ liệu nhưng chưa add vào CSDL
                Create(chuong);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenChuong bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateChuong(Chuong chuong)
        {
            /*Bắt lỗi [ID]*/
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
            /*End*/

            /*Bắt lỗi [Tên chương]*/
            if (FindByCondition(t => t.TenChuong.Equals(chuong.TenChuong) 
                                && t.TruyenID.Equals(chuong.TruyenID)
                                && t.ChuongID != chuong.ChuongID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên chương bị trùng"
                };
            }
            /*End*/

            var chuongOld = FindByCondition(m => m.ChuongID.Equals(chuong.ChuongID)).FirstOrDefault();
            chuong.ThoiGianCapNhat = (chuong.ThoiGianCapNhat == "" || chuong.ThoiGianCapNhat == null) ? chuongOld.ThoiGianCapNhat: chuong.ThoiGianCapNhat;
            chuong.LuotXem = chuong.LuotXem == 0 ? chuongOld.LuotXem: chuong.LuotXem;

            //Tạo bản ghi mới nhưng chưa update vào CSDL
            Update(chuong);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa chương thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteChuong(Chuong chuong)
        {
            chuong.TinhTrang = true;
            Update(chuong);
            
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa chương thành công" };
        }

        //Lấy danh sách các chương không bị xóa
        public async Task<IEnumerable<Chuong>> GetAllChuongsAsync()
        {
            return await FindAll()
                .Where(chuong => !chuong.TinhTrang)
                .OrderBy(chuong => chuong.ChuongID)
                .Include(m => m.Truyen)
                .Include(m => m.NoiDungChuongs)
                .ToListAsync();
        }

        public async Task<Chuong> GetChuongByIdAsync(int chuongId)
        {
            return await FindByCondition(chuong => chuong.ChuongID.Equals(chuongId) && !chuong.TinhTrang)
                    .FirstOrDefaultAsync();
        }

        public async Task<Chuong> GetChuongByDetailAsync(int chuongId)
        {
            return await FindByCondition(chuong => chuong.ChuongID.Equals(chuongId) && !chuong.TinhTrang)
                .Include(chuong => chuong.Truyen)
                .Include(chuong => chuong.NoiDungChuongs)
                .Include(chuong => chuong.BinhLuans)
                    .ThenInclude(b => b.User)
                .FirstOrDefaultAsync();
        }
    }
}
