using DataAccessLayer;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class TacGiaRepository : RepositoryBase<TacGia>, ITacGiaRepository
    {
        public TacGiaRepository(RepositoryContext repositoryContext)
            :base(repositoryContext)
        {
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenTacGia bị trùng, null: thêm thành công
        public ResponseDetails CreateTacGia(IEnumerable<TacGia> tacGias)
        {
            foreach (var tacGia in tacGias)
            {
                if (FindByCondition(t => t.TenTacGia.Equals(tacGia.TenTacGia)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên tác giả bị trùng",
                        Value = tacGia.TenTacGia
                    };
                }

                Create(tacGia);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateTacGia(TacGia tacGia)
        {
            if (FindByCondition(t => t.TenTacGia.Equals(tacGia.TenTacGia)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên tác giả bị trùng",
                    Value = tacGia.TenTacGia
                };
            }
            
            Update(tacGia);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa tác giả thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteTacGia(TacGia tacGia)
        {
            tacGia.TinhTrang = true;
            Update(tacGia);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa tác giả thành công" };
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
