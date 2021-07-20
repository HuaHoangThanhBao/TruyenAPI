using CoreLibrary;
using CoreLibrary.Helpers;
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
            //if (ValidationExtensions.isSpecialChar(binhLuan.NoiDung))
            //{
            //    return new ResponseDetails()
            //    {
            //        StatusCode = ResponseCode.Error,
            //        Message = "Không được chứa ký tự đặc biệt",
            //        Value = binhLuan.NoiDung.ToString()
            //    };
            //}
            /*End*/

            /*Bắt lỗi [ID]*/
            var userRepo = new UserRepository(_context);
            var chuongRepo = new ChuongRepository(_context);
            var truyenRepo = new TruyenRepository(_context);

            if (!userRepo.FindByCondition(t => t.UserID == binhLuan.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID User không tồn tại",
                    Value = binhLuan.UserID.ToString()
                };
            }

            if(binhLuan.ChuongID == null && binhLuan.TruyenID == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Vui lòng cung cấp ChuongID hoặc TruyenID",
                    Value = ""
                };
            }

            if (binhLuan.ChuongID != null && !chuongRepo.FindByCondition(t => t.ChuongID == binhLuan.ChuongID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID Chương không tồn tại",
                    Value = binhLuan.ChuongID.ToString()
                };
            }

            if (binhLuan.TruyenID != null && !truyenRepo.FindByCondition(t => t.TruyenID == binhLuan.TruyenID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID Truyện không tồn tại",
                    Value = binhLuan.ChuongID.ToString()
                };
            }

            if (binhLuan.NoiDung == "" || binhLuan.NoiDung == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Nội dung bình luận không được để trống",
                    Value = ""
                };
            }
            /*End*/

            binhLuan.NgayBL = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            Create(binhLuan);
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenBinhLuan bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateBinhLuan(BinhLuan binhLuan)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            //if (ValidationExtensions.isSpecialChar(binhLuan.NoiDung))
            //{
            //    return new ResponseDetails()
            //    {
            //        StatusCode = ResponseCode.Error,
            //        Message = "Không được chứa ký tự đặc biệt",
            //        Value = binhLuan.NoiDung.ToString()
            //    };
            //}
            /*End*/
            
            /*Bắt lỗi [ID]*/
            var userRepo = new UserRepository(_context);
            var chuongRepo = new ChuongRepository(_context);
            var truyenRepo = new TruyenRepository(_context);

            if (!userRepo.FindByCondition(t => t.UserID == binhLuan.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID User không tồn tại",
                    Value = binhLuan.UserID.ToString()
                };
            }


            if (binhLuan.ChuongID == null && binhLuan.TruyenID == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Vui lòng cung cấp ChuongID hoặc TruyenID",
                    Value = ""
                };
            }

            if (binhLuan.ChuongID != null && !chuongRepo.FindByCondition(t => t.ChuongID == binhLuan.ChuongID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID Chương không tồn tại",
                    Value = binhLuan.ChuongID.ToString()
                };
            }

            if (binhLuan.TruyenID != null && !truyenRepo.FindByCondition(t => t.TruyenID == binhLuan.TruyenID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID Truyện không tồn tại",
                    Value = binhLuan.ChuongID.ToString()
                };
            }

            if (binhLuan.NoiDung == "" || binhLuan.NoiDung == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Nội dung bình luận không được để trống",
                    Value = ""
                };
            }
            /*End*/

            var binhLuanOld = FindByCondition(m => m.BinhLuanID.Equals(binhLuan.BinhLuanID)).FirstOrDefault();
            binhLuan.NgayBL = (binhLuan.NgayBL == "" || binhLuan.NgayBL == null) ? binhLuanOld.NgayBL : binhLuan.NgayBL;

            Update(binhLuan);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa bình luận thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteBinhLuan(BinhLuan binhLuan)
        {
            binhLuan.TinhTrang = true;
            Update(binhLuan);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa bình luận thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<BinhLuan>> GetAllBinhLuansAsync()
        {
            return await FindAll()
                .Where(binhLuan => !binhLuan.TinhTrang)
                .Include(binhLuan => binhLuan.Chuong)
                    .ThenInclude(b => b.Truyen)
                .Include(binhLuan => binhLuan.User)
                .OrderBy(binhLuan => binhLuan.BinhLuanID)
                .ToListAsync();
        }

        public async Task<BinhLuan> GetBinhLuanByIdAsync(int binhLuanId)
        {
            return await FindByCondition(binhLuan => binhLuan.BinhLuanID.Equals(binhLuanId) && !binhLuan.TinhTrang)
                    .FirstOrDefaultAsync();
        }

        public async Task<BinhLuan> GetBinhLuanByDetailAsync(int binhLuanId)
        {
            return await FindByCondition(binhLuan => binhLuan.BinhLuanID.Equals(binhLuanId) && !binhLuan.TinhTrang)
                .Include(binhLuan => binhLuan.Chuong)
                .Include(binhLuan => binhLuan.User)
                .FirstOrDefaultAsync();
        }


        //Lấy tất cả bình luận có trong bảng (theo thứ tự giảm dần ID)
        public async Task<PagedList<BinhLuan>> GetBinhLuanForPagination(BinhLuanParameters binhLuanParameters)
        {
            return await PagedList<BinhLuan>.ToPagedList(FindAll().Where(m => !m.TinhTrang)
                .Include(m => m.User)
                .Include(m => m.Truyen)
                .OrderByDescending(on => on.BinhLuanID),
                binhLuanParameters.PageNumber,
                binhLuanParameters.PageSize);
        }

        //Lấy số lượng bình luận mới nhất có trong bảng (theo thứ tự giảm dần ID)
        public async Task<PagedList<BinhLuan>> GetBinhLuanLastestForPagination(BinhLuanParameters binhLuanParameters)
        {
            return await PagedList<BinhLuan>.ToPagedList(FindAll().Where(m => !m.TinhTrang)
                .Include(m => m.User)
                .Include(m => m.Truyen)
                .Include(m => m.Chuong)
                    .ThenInclude(m => m.Truyen)
                .OrderByDescending(on => on.BinhLuanID).Take(binhLuanParameters.PageSize),
                binhLuanParameters.PageNumber,
                binhLuanParameters.PageSize);
        }

        //Lấy tất cả bình luận của truyện theo [ID] không tính các bình luận của chương trong nó
        public async Task<PagedList<BinhLuan>> GetBinhLuanOfTruyenForPagination(int truyenID, BinhLuanParameters binhLuanParameters)
        {
            return await PagedList<BinhLuan>.ToPagedList(

                (from m in _context.BinhLuans
                 where m.TruyenID == truyenID && m.TruyenID != null && !m.TinhTrang
                 select m)
                 .Include(m => m.User)
                 .Include(m => m.Truyen)
                 .Take(binhLuanParameters.PageSize)
                 .OrderByDescending(on => on.BinhLuanID)
                
                 ,
                binhLuanParameters.PageNumber,
                binhLuanParameters.PageSize);
        }

        //Lấy tất cả bình luận của chương theo [ID]
        public async Task<PagedList<BinhLuan>> GetBinhLuanOfChuongForPagination(int chuongID, BinhLuanParameters binhLuanParameters)
        {

            return await PagedList<BinhLuan>.ToPagedList(

                (from m in _context.BinhLuans
                 where m.ChuongID == chuongID && m.ChuongID != null && !m.TinhTrang
                 select m)
                 .Include(m => m.User)
                 .Include(m => m.Chuong)
                 .Take(binhLuanParameters.PageSize)
                 .OrderByDescending(on => on.BinhLuanID)

                 ,
                binhLuanParameters.PageNumber,
                binhLuanParameters.PageSize);
        }
    }
}
