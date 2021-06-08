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
        public ResponseDetails CreateNoiDungChuong(IEnumerable<NoiDungChuong> noiDungChuongs)
        {
            var truyenRepo = new TruyenRepository(_context);

            foreach (var nd in noiDungChuongs)
            {
                /*Bắt lỗi [ID]*/
                if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(nd.ChuongID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "ID chương không tồn tại",
                        Value = nd.ChuongID.ToString()
                    };
                }
                /*End*/

                /*Bắt lỗi [Tên hình ảnh]*/
                if (FindByCondition(t => t.HinhAnh.Equals(nd.HinhAnh) && t.ChuongID.Equals(nd.ChuongID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Chương đã tồn tại hình ảnh này",
                        Value = nd.HinhAnh
                    };
                }
                /*End*/

                //Tạo dữ liệu nhưng chưa add vào CSDL
                Create(nd);
            }

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Thêm nội dung chương thành công"};
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateNoiDungChuong(NoiDungChuong nd)
        {
            /*Bắt lỗi [ID]*/
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
            /*End*/

            /*Bắt lỗi [Tên hình ảnh]*/
            if (FindByCondition(t => t.HinhAnh.Equals(nd.HinhAnh) && t.ChuongID.Equals(nd.ChuongID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Chương đã tồn tại hình ảnh này",
                    Value = nd.HinhAnh
                };
            }
            /*End*/

            //Tạo bản ghi mới nhưng chưa update vào CSDL
            Update(nd);

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa nội dung chương thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteNoiDungChuong(NoiDungChuong noiDungChuong)
        {
            Delete(noiDungChuong);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa nội dung chương thành công" };
        }

        //Lấy danh sách các nội dung chương
        public async Task<IEnumerable<NoiDungChuong>> GetAllNoiDungChuongsAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.ChuongID)
                .ToListAsync();
        }

        //Lấy nội dung chương = [id]
        public async Task<NoiDungChuong> GetNoiDungChuongByChuongIdAsync(int noiDungId)
        {
            return await FindByCondition(chuong => chuong.NoiDungChuongID.Equals(noiDungId))
                .FirstOrDefaultAsync();
        }
    }
}
