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
    public class PhuLucRepository : RepositoryBase<PhuLuc>, IPhuLucRepository
    {
        private RepositoryContext _context;

        public PhuLucRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public IEnumerable<PhuLuc> TheLoaisInPhuLuc(int theLoaiId)
        {
            return FindByCondition(a => a.TheLoaiID.Equals(theLoaiId)).ToList();
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: false = TruyenID hoặc TheLoaiID không tồn tại, true: thêm thành công
        public bool CreatePhuLuc(IEnumerable<PhuLuc> phuLucs)
        {
            foreach (var phuLuc in phuLucs)
            {
                var truyenRepo = new TruyenRepository(_context);
                var theLoaiRepo = new TheLoaiRepository(_context);

                if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(phuLuc.TruyenID)).Any()) return false;
                if (!theLoaiRepo.FindByCondition(t => t.TheLoaiID.Equals(phuLuc.TheLoaiID)).Any()) return false;


                Create(phuLuc);
            }
            return true;
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public bool UpdatePhuLuc(PhuLuc phuLuc)
        {
            var truyenRepo = new TruyenRepository(_context);
            var theLoaiRepo = new TheLoaiRepository(_context);

            if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(phuLuc.TruyenID)).Any()) return false;
            if (!theLoaiRepo.FindByCondition(t => t.TheLoaiID.Equals(phuLuc.TheLoaiID)).Any()) return false;

            Update(phuLuc);
            return true;
        }

        //Xóa logic
        public void DeletePhuLuc(PhuLuc phuLuc)
        {
            Delete(phuLuc);
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<PhuLuc>> GetAllPhuLucsAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.TruyenID)
                .ToListAsync();
        }

        public async Task<PhuLuc> GetPhuLucByIdAsync(int phuLucId)
        {
            return await FindByCondition(phuLuc => phuLuc.PhuLucID.Equals(phuLucId))
                    .FirstOrDefaultAsync();
        }
    }
}
