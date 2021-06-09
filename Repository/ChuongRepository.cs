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
                /*Bắt lỗi ký tự đặc biệt*/
                if (ValidationExtensions.isSpecialChar(chuong.TenChuong))
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Không được chứa ký tự đặc biệt",
                        Value = chuong.TenChuong.ToString()
                    };
                }
                /*End*/

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
                if (chuong.TenChuong == "" || chuong.TenChuong == null)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên chương không được để trống",
                        Value = chuong.TenChuong
                    };
                }

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

                /*Bắt lỗi [Thời gian cập nhật]*/
                if (chuong.ThoiGianCapNhat == null)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Thời gian cập nhật không được để trống",
                        Value = chuong.TenChuong
                    };
                }
                /*End*/

                //Mặc định khi thêm chương thì chương đó sẽ ở trạng thái chờ duyệt (=-1)
                chuong.TrangThai = -1;

                //Tạo dữ liệu nhưng chưa add vào CSDL
                Create(chuong);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenChuong bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateChuong(Chuong chuong)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(chuong.TenChuong))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Không được chứa ký tự đặc biệt",
                    Value = chuong.TenChuong.ToString()
                };
            }
            /*End*/

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
            if (chuong.TenChuong == "" || chuong.TenChuong == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên chương không được để trống",
                    Value = chuong.TenChuong
                };
            }

            if (FindByCondition(t => t.TenChuong.Equals(chuong.TenChuong) && t.TruyenID.Equals(chuong.TruyenID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên chương bị trùng"
                };
            }
            /*End*/

            /*Bắt lỗi [Thời gian cập nhật]*/
            if (chuong.ThoiGianCapNhat == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Thời gian cập nhật không được để trống",
                    Value = chuong.TenChuong
                };
            }
            /*End*/

            //Tạo bản ghi mới nhưng chưa update vào CSDL
            Update(chuong);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa chương thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteChuong(Chuong chuong)
        {
            //Khi xóa chương thì xóa luôn những [Nội dung chương] kèm theo
            var noiDungChuongRepo = FindByCondition(t => t.ChuongID.Equals(chuong.ChuongID));
            foreach (var noiDung in noiDungChuongRepo)
            {
                Delete(noiDung);
            }

            //Tạo bản ghi mới nhưng chưa update vào CSDL
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
                .Include(ac => ac.NoiDungChuongs)
                .Include(a => a.BinhLuans)
                    .ThenInclude(b => b.User)
                .FirstOrDefaultAsync();
        }
    }
}
