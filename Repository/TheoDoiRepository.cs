using CoreLibrary;
using CoreLibrary.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class TheoDoiRepository : RepositoryBase<TheoDoi>, ITheoDoiRepository
    {
        private RepositoryContext _context;
        public TheoDoiRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenTheoDoi bị trùng, null: thêm thành công
        public ResponseDetails CreateTheoDoi(TheoDoi theoDoi)
        {
            var userRepo = new UserRepository(_context);
            var truyenRepo = new TruyenRepository(_context);
            if (!userRepo.FindByCondition(t => t.UserID == theoDoi.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID User không tồn tại",
                    Value = theoDoi.UserID.ToString()
                };
            }
            if (!truyenRepo.FindByCondition(t => t.TruyenID == theoDoi.TruyenID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID Truyện không tồn tại",
                    Value = theoDoi.TruyenID.ToString()
                };
            }

            Create(theoDoi);
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTheoDoi bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateTheoDoi(TheoDoi theoDoi)
        {
            var userRepo = new UserRepository(_context);
            var truyenRepo = new TruyenRepository(_context);
            if (!userRepo.FindByCondition(t => t.UserID == theoDoi.UserID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID User không tồn tại",
                    Value = theoDoi.UserID.ToString()
                };
            }
            if (!truyenRepo.FindByCondition(t => t.TruyenID == theoDoi.TruyenID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "ID Truyện không tồn tại",
                    Value = theoDoi.TruyenID.ToString()
                };
            }

            Update(theoDoi);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa TheoDoi thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteTheoDoi(TheoDoi TheoDoi)
        {
            Delete(TheoDoi);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa TheoDoi thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<TheoDoi>> GetAllTheoDoisAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.TheoDoiID)
                .ToListAsync();
        }

        public async Task<TheoDoi> GetTheoDoiByIdAsync(int TheoDoiId)
        {
            return await FindByCondition(TheoDoi => TheoDoi.TheoDoiID.Equals(TheoDoiId))
                    .FirstOrDefaultAsync();
        }
    }
}
