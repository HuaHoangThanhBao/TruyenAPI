﻿using DataAccessLayer;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        public ResponseDetails CreatePhuLuc(IEnumerable<PhuLuc> phuLucs)
        {
            foreach (var phuLuc in phuLucs)
            {
                var truyenRepo = new TruyenRepository(_context);
                var theLoaiRepo = new TheLoaiRepository(_context);

                if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(phuLuc.TruyenID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "ID truyện không tồn tại",
                        Value = phuLuc.TruyenID.ToString()
                    };
                }
                if (!theLoaiRepo.FindByCondition(t => t.TheLoaiID.Equals(phuLuc.TheLoaiID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "ID thể loại không tồn tại",
                        Value = phuLuc.TruyenID.ToString()
                    };
                }


                Create(phuLuc);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success};
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdatePhuLuc(PhuLuc phuLuc)
        {
            var truyenRepo = new TruyenRepository(_context);
            var theLoaiRepo = new TheLoaiRepository(_context);

            if (!truyenRepo.FindByCondition(t => t.TruyenID.Equals(phuLuc.TruyenID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID truyện không tồn tại",
                    Value = phuLuc.TruyenID.ToString()
                };
            }
            if (!theLoaiRepo.FindByCondition(t => t.TheLoaiID.Equals(phuLuc.TheLoaiID)).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID thể loại không tồn tại",
                    Value = phuLuc.TruyenID.ToString()
                };
            }

            Update(phuLuc);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa phụ lục thành công" };
        }

        //Xóa logic
        public ResponseDetails DeletePhuLuc(PhuLuc phuLuc)
        {
            Delete(phuLuc);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa phụ lục thành công" };
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
