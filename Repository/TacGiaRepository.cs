using DataAccessLayer;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.Extensions;

namespace Repository
{
    public class TacGiaRepository : RepositoryBase<TacGia>, ITacGiaRepository
    {
        private RepositoryContext _context;

        public TacGiaRepository(RepositoryContext repositoryContext)
            :base(repositoryContext)
        {
            _context = repositoryContext;
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenTacGia bị trùng, null: thêm thành công
        public ResponseDetails CreateTacGia(IEnumerable<TacGia> tacGias)
        {
            foreach (var tacGia in tacGias)
            {
                /*Bắt lỗi ký tự đặc biệt*/
                if (ValidationExtensions.isSpecialChar(tacGia.TenTacGia))
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Không được chứa ký tự đặc biệt",
                        Value = tacGia.TenTacGia.ToString()
                    };
                }
                /*End*/

                /*Bắt lỗi [Tên tác giả]*/
                if (tacGia.TenTacGia == "" || tacGia.TenTacGia == null)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên tác giả không được để trống",
                        Value = tacGia.TenTacGia
                    };
                }

                if (FindByCondition(t => t.TenTacGia.Equals(tacGia.TenTacGia)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên tác giả bị trùng",
                        Value = tacGia.TenTacGia
                    };
                }
                /*End*/

                Create(tacGia);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateTacGia(TacGia tacGia)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(tacGia.TenTacGia))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Không được chứa ký tự đặc biệt",
                    Value = tacGia.TenTacGia.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [Tên tác giả]*/
            if (tacGia.TenTacGia == "" || tacGia.TenTacGia == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên tác giả không được để trống",
                    Value = tacGia.TenTacGia
                };
            }

            if (FindByCondition(t => t.TenTacGia.Equals(tacGia.TenTacGia)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên tác giả bị trùng",
                    Value = tacGia.TenTacGia
                };
            }
            /*End*/

            Update(tacGia);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa tác giả thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteTacGia(TacGia tacGia)
        {
            //Kiểm tra [id tác giả] hiện tại có nằm trong [truyện] không? Nếu không thì cho phép xóa
            var truyenRepo = new TruyenRepository(_context);
            if (!truyenRepo.FindByCondition(t => t.TacGiaID.Equals(tacGia.TacGiaID) && !t.TinhTrang).Any())
            {
                tacGia.TinhTrang = true;
                Update(tacGia);
                return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa tác giả thành công" };
            }
            else
            {
                return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Tác giả này đang tồn tại ở Truyện" };
            }
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<TacGia>> GetAllTacGiasAsync()
        {
            return await FindAll()
                .Where(ow => !ow.TinhTrang)
                .OrderBy(ow => ow.TenTacGia)
                .ToListAsync();
        }

        public async Task<TacGia> GetTacGiaByIdAsync(int tacGiaId)
        {
            return await FindByCondition(tacGia => tacGia.TacGiaID.Equals(tacGiaId))
                    .FirstOrDefaultAsync();
        }

        public async Task<TacGia> GetTacGiaByDetailAsync(int tacGiaId)
        {
            return await FindByCondition(tacGia => tacGia.TacGiaID.Equals(tacGiaId))
                .Include(ac => ac.Truyens)
                .FirstOrDefaultAsync();
        }
    }
}
