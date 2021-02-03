using Contracts;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public TacGia CreateTacGia(IEnumerable<TacGia> tacGias)
        {
            foreach (var tacGia in tacGias)
            {
                if (FindByCondition(t => t.TenTacGia.Equals(tacGia.TenTacGia)).Any()) return tacGia;

                Create(tacGia);
            }
            return null;
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public bool UpdateTacGia(TacGia tacGia)
        {
            if (FindByCondition(t => t.TenTacGia.Equals(tacGia.TenTacGia)).Any()) return false;
            
            Update(tacGia);
            return true;
        }

        //Xóa logic
        public void DeleteTacGia(TacGia tacGia)
        {
            tacGia.TinhTrang = true;
            Update(tacGia);
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<TacGia>> GetAllTacGiasAsync()
        {
            return await FindAll()
                .Where(ow => !ow.TinhTrang)
                .OrderBy(ow => ow.TenTacGia)
                .ToListAsync();
        }

        public async Task<TacGia> GetTacGiaByIdAsync(Guid tacGiaId)
        {
            return await FindByCondition(tacGia => tacGia.TacGiaID.Equals(tacGiaId))
                    .FirstOrDefaultAsync();
        }

        public async Task<TacGia> GetTacGiaByDetailAsync(Guid tacGiaId)
        {
            return await FindByCondition(tacGia => tacGia.TacGiaID.Equals(tacGiaId))
                .Include(ac => ac.Truyens)
                .FirstOrDefaultAsync();
        }
    }
}
