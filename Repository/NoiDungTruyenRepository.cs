using DataAccessLayer;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class NoiDungTruyenRepository : RepositoryBase<NoiDungTruyen>, INoiDungTruyenRepository
    {
        private RepositoryContext _context;

        public NoiDungTruyenRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: false = TruyenID hoặc TheLoaiID không tồn tại, true: thêm thành công
        public ResponseDetails CreateNoiDungTruyen(NoiDungTruyen nd)
        {
            if (FindByCondition(t => t.HinhAnh.Equals(nd.HinhAnh) && t.TruyenID.Equals(nd.TruyenID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Truyện này đã tồn tại hình ảnh truyện này",
                    Value = nd.HinhAnh
                };
            }

            var truyenRepo = new TruyenRepository(_context);
            if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(nd.TruyenID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID truyện không tồn tại",
                    Value = nd.TruyenID.ToString()
                };
            }

            Create(nd);

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Thêm nội dung truyện thành công", Value = nd.HinhAnh };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateNoiDungTruyen(NoiDungTruyen nd)
        {
            if (FindByCondition(t => t.HinhAnh.Equals(nd.HinhAnh) && t.TruyenID.Equals(nd.TruyenID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Truyện này đã tồn tại hình ảnh truyện này",
                    Value = nd.HinhAnh
                };
            }

            var truyenRepo = new TruyenRepository(_context);
            if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(nd.TruyenID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID truyện không tồn tại",
                    Value = nd.TruyenID.ToString()
                };
            }

            Update(nd);

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa nội dung truyện thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteNoiDungTruyen(int truyenId)
        {
            var truyenVsNoiDungTruyenRepo = FindByCondition(t => t.TruyenID.Equals(truyenId));

            foreach (var noiDung in truyenVsNoiDungTruyenRepo)
            {
                Delete(noiDung);
            }

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa nội dung truyện thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<NoiDungTruyen>> GetAllNoiDungTruyensAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.TruyenID)
                .ToListAsync();
        }

        public async Task<NoiDungTruyen> GetNoiDungTruyenByTruyenIdAsync(int noiDungId, int truyenId)
        {
            return await FindByCondition(truyen => truyen.TruyenID.Equals(truyenId) && truyen.NoiDungTruyenID.Equals(noiDungId))
                .FirstOrDefaultAsync();
        }
    }
}
