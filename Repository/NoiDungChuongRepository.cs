using DataAccessLayer;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class NoiDungChuongRepository : RepositoryBase<NoiDungChuong>, INoiDungChuongRepository
    {
        private RepositoryContext _context;

        public NoiDungChuongRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: false = TruyenID hoặc TheLoaiID không tồn tại, true: thêm thành công
        public ResponseDetails CreateNoiDungChuong(NoiDungChuong nd)
        {
            if (FindByCondition(t => t.HinhAnh.Equals(nd.HinhAnh) && t.ChuongID.Equals(nd.ChuongID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Chương đã tồn tại hình ảnh này",
                    Value = nd.HinhAnh
                };
            }

            var truyenRepo = new TruyenRepository(_context);
            if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(nd.ChuongID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID chương không tồn tại",
                    Value = nd.ChuongID.ToString()
                };
            }

            Create(nd);

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Thêm nội dung chương thành công", Value = nd.HinhAnh };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateNoiDungChuong(NoiDungChuong nd)
        {
            if (FindByCondition(t => t.HinhAnh.Equals(nd.HinhAnh) && t.ChuongID.Equals(nd.ChuongID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Chương đã tồn tại hình ảnh này",
                    Value = nd.HinhAnh
                };
            }

            var truyenRepo = new TruyenRepository(_context);
            if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(nd.ChuongID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID chương không tồn tại",
                    Value = nd.ChuongID.ToString()
                };
            }

            Update(nd);

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa nội dung chương thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteNoiDungChuong(int truyenId)
        {
            var chuongVsNoiDungChuongRepo = FindByCondition(t => t.ChuongID.Equals(truyenId));

            foreach (var noiDung in chuongVsNoiDungChuongRepo)
            {
                Delete(noiDung);
            }

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa nội dung chương thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<NoiDungChuong>> GetAllNoiDungChuongsAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.ChuongID)
                .ToListAsync();
        }

        public async Task<NoiDungChuong> GetNoiDungChuongByChuongIdAsync(int noiDungId, int truyenId)
        {
            return await FindByCondition(truyen => truyen.ChuongID.Equals(truyenId) && truyen.ChuongID.Equals(noiDungId))
                .FirstOrDefaultAsync();
        }
    }
}
