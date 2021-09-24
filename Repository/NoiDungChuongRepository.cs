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
            /*Kiểm tra xem chuỗi json nhập vào có bị trùng tên chương không*/
            foreach (var dup in noiDungChuongs.GroupBy(p => p.HinhAnh))
            {
                if (dup.Count() - 1 > 0)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Chuỗi json nhập vào bị trùng đường dẫn ảnh",
                        Value = dup.Key.ToString()
                    };
                }
            }
            /*End*/

            var chuongRepo = new ChuongRepository(_context);

            foreach (var nd in noiDungChuongs)
            {
                /*Bắt lỗi [ID]*/
                if (!chuongRepo.FindByCondition(t => t.ChuongID.Equals(nd.ChuongID)).Any())
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

                if (FindByCondition(t => t.HinhAnh.Equals(nd.HinhAnh)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Hình ảnh này đã nằm trong 1 chương khác",
                        Value = nd.HinhAnh
                    };
                }
                /*End*/

                //Tạo dữ liệu nhưng chưa add vào CSDL
                Create(nd);
                _context.SaveChanges();
            }

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Thêm nội dung chương thành công"};
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateNoiDungChuong(NoiDungChuong nd)
        {
            /*Bắt lỗi [ID]*/
            var chuongRepo = new ChuongRepository(_context);
            if (!chuongRepo.FindByCondition(t => t.ChuongID.Equals(nd.ChuongID)).Any())
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
            if (FindByCondition(t => t.HinhAnh.Equals(nd.HinhAnh) && !t.ChuongID.Equals(nd.ChuongID)).Any())
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
            noiDungChuong.TinhTrang = true;
            Update(noiDungChuong);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa nội dung chương thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteMultipleNoiDungChuong(IEnumerable<NoiDungChuong> noiDungChuongs)
        {
            /*Kiểm tra xem chuỗi json nhập vào có bị trùng tên chương không*/
            foreach (var dup in noiDungChuongs.GroupBy(p => p.HinhAnh))
            {
                if (dup.Count() - 1 > 0)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Chuỗi json nhập vào bị trùng đường dẫn ảnh",
                        Value = dup.Key.ToString()
                    };
                }
            }
            /*End*/

            var chuongRepo = new ChuongRepository(_context);

            foreach (var nd in noiDungChuongs)
            {
                /*Bắt lỗi [ID]*/
                if (!chuongRepo.FindByCondition(t => t.ChuongID.Equals(nd.ChuongID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "ID chương không tồn tại",
                        Value = nd.ChuongID.ToString()
                    };
                }
                /*End*/

                Update(nd);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa các nội dung chương thành công" };
        }

        //Lấy danh sách các nội dung chương
        public async Task<IEnumerable<NoiDungChuong>> GetAllNoiDungChuongsAsync()
        {
            return await FindAll()
                .Where(noiDungChuong => !noiDungChuong.TinhTrang)
                .OrderBy(noiDungChuong => noiDungChuong.ChuongID)
                .ToListAsync();
        }

        //Lấy nội dung chương = [id]
        public async Task<NoiDungChuong> GetNoiDungChuongByIdAsync(int noiDungId)
        {
            return await FindByCondition(noiDungChuong => noiDungChuong.NoiDungChuongID.Equals(noiDungId) && !noiDungChuong.TinhTrang)
                .FirstOrDefaultAsync();
        }
    }
}
