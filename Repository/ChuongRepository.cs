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

                var found = FindByCondition(m => m.TruyenID.Equals(chuong.TruyenID));
                if (found.Count() > 0)
                {
                    chuong.STT = found.Max(m => m.STT) + 1;
                }
                else chuong.STT = 1;
                chuong.ThoiGianCapNhat = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                Random r = new Random();
                chuong.LuotXem = r.Next(5000, 15000);

                //Tạo dữ liệu nhưng chưa add vào CSDL
                Create(chuong);
                _context.SaveChanges();
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

            chuong.TenChuong = chuong.TenChuong.ToLower();
            var chuongOld = FindByCondition(m => m.ChuongID.Equals(chuong.ChuongID)).FirstOrDefault();
            chuong.ThoiGianCapNhat = (chuong.ThoiGianCapNhat == "" || chuong.ThoiGianCapNhat == null) ? chuongOld.ThoiGianCapNhat: DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
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

        public ResponseDetails DeleteHardChuong(Chuong chuong)
        {
            Delete(chuong);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa chương hard code thành công" };
        }

        //Lấy danh sách các chương không bị xóa
        public async Task<IEnumerable<Chuong>> GetAllChuongsAsync()
        {
            return await FindAll()
                .Where(chuong => !chuong.TinhTrang)
                .OrderBy(chuong => chuong.STT)
                .Include(m => m.Truyen)
                .Include(m => m.NoiDungChuongs)
                .ToListAsync();
        }

        public async Task<Chuong> GetChuongByIdAsync(int chuongId)
        {
            return await FindByCondition(chuong => chuong.ChuongID.Equals(chuongId) && !chuong.TinhTrang)
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Chuong>> GetAllChuongsByTruyenIdAsync(int truyenID)
        {
            return await FindAll()
                .Where(chuong => chuong.TruyenID.Equals(truyenID) && !chuong.TinhTrang)
                .OrderBy(chuong => chuong.STT)
                .ToListAsync();
        }

        //Chưa tạo api cho hàm này
        public async Task<Chuong> GetNewestChuongByTruyenIdAsync(int truyenID)
        {
            return await FindByCondition(chuong => chuong.TruyenID.Equals(truyenID) && !chuong.TinhTrang)
                .OrderByDescending(chuong => chuong.STT)
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
