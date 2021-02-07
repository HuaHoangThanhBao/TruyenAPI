using DataAccessLayer;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class TruyenRepository : RepositoryBase<Truyen>, ITruyenRepository
    {
        private RepositoryContext _context;
        public TruyenRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenTruyen bị trùng, null: thêm thành công
        public ResponseDetails CreateTruyen(IEnumerable<Truyen> truyens)
        {
            foreach (var truyen in truyens)
            {
                if (FindByCondition(t => t.TenTruyen.Equals(truyen.TenTruyen)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên truyện bị trùng",
                        Value = truyen.TenTruyen
                    };
                }


                var tacGiaRepo = new TacGiaRepository(_context);
                if (!tacGiaRepo.FindByCondition(t => t.TacGiaID.Equals(truyen.TacGiaID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "ID tác giả không tồn tại",
                        Value = truyen.TacGiaID.ToString()
                    };
                }

                Create(truyen);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTruyen bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateTruyen(Truyen truyen)
        {
            if (FindByCondition(t => t.TenTruyen.Equals(truyen.TenTruyen)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên truyện bị trùng"
                };
            }

            var tacGiaRepo = new TacGiaRepository(_context);
            if (!tacGiaRepo.FindByCondition(t => t.TacGiaID.Equals(truyen.TacGiaID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID tác giả không tồn tại",
                    Value = truyen.TacGiaID.ToString()
                };
            }

            Update(truyen);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa truyện thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteTruyen(Truyen truyen)
        {
            truyen.TinhTrang = true;
            Update(truyen);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa truyện thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<Truyen>> GetAllTruyensAsync()
        {
            return await FindAll()
                .Where(ow => !ow.TinhTrang)
                .OrderBy(ow => ow.TenTruyen)
                .ToListAsync();
        }

        public async Task<Truyen> GetTruyenByIdAsync(int truyenId)
        {
            return await FindByCondition(truyen => truyen.TruyenID.Equals(truyenId))
                    .FirstOrDefaultAsync();
        }

        public async Task<Truyen> GetTruyenByDetailAsync(int truyenId)
        {
            return await FindByCondition(truyen => truyen.TruyenID.Equals(truyenId))
                .Include(ac => ac.BinhLuans)
                .Include(ac => ac.Chuongs)
                .Include(ac => ac.PhuLucs)
                .Include(ac => ac.TheoDois)
                .Include(ac => ac.NoiDungTruyens)
                .FirstOrDefaultAsync();
        }
    }
}
