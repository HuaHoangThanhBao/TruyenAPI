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
    public class TruyenRepository : RepositoryBase<Truyen>, ITruyenRepository
    {
        public TruyenRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        //public IEnumerable<TheLoai> AccountsByOwner(int ownerId)
        //{
        //    return FindByCondition(a => a.TheLoaiID.Equals(ownerId)).ToList();
        //}

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenTruyen bị trùng, null: thêm thành công
        public Truyen CreateTruyen(IEnumerable<Truyen> truyens)
        {
            foreach (var truyen in truyens)
            {
                if (FindByCondition(t => t.TenTruyen.Equals(truyen.TenTruyen)).Any()) return truyen;

                Create(truyen);
            }
            return null;
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public bool UpdateTruyen(Truyen truyen)
        {
            if (FindByCondition(t => t.TenTruyen.Equals(truyen.TenTruyen)).Any()) return false;

            Update(truyen);
            return true;
        }

        //Xóa logic
        public void DeleteTruyen(Truyen truyen)
        {
            truyen.TinhTrang = true;
            Update(truyen);
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
                .FirstOrDefaultAsync();
        }
    }
}
