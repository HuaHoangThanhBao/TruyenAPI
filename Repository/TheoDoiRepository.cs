﻿using CoreLibrary;
using CoreLibrary.Helpers;
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
            if (FindByCondition(t => t.UserID == theoDoi.UserID && t.TruyenID == theoDoi.TruyenID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Bạn đã theo dõi truyện này",
                    Value = theoDoi.TruyenID.ToString()
                };
            }

            theoDoi.ThoiGian = DateTime.Now;
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
            if (FindByCondition(t => t.UserID == theoDoi.UserID && t.TruyenID == theoDoi.TruyenID & t.TheoDoiID != theoDoi.TheoDoiID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Bạn đã theo dõi truyện này",
                    Value = theoDoi.TruyenID.ToString()
                };
            }

            theoDoi.ThoiGian = DateTime.Now;
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

        public async Task<IEnumerable<TheoDoi>> GetTheoDoiByUserIdAsync(Guid userId)
        {
            return await FindAll()
                .Where(m => m.UserID.Equals(userId))
                .Include(m => m.Truyen)
                .OrderBy(ow => ow.TheoDoiID)
                .ToListAsync();
        }

        public async Task<TheoDoi> GetTheoDoiByIdAsync(int theoDoiId)
        {
            return await FindByCondition(theoDoi => theoDoi.TheoDoiID.Equals(theoDoiId))
                    .FirstOrDefaultAsync();
        }

        public int GetLuotTheoDoiByTruyenID(int truyenID)
        {
            return FindAll().Where(m => m.TruyenID.Equals(truyenID)).Count();
        }

        public async Task<TheoDoi> GetTheoDoiByUserIdAndTruyenIdAsync(string userID, int truyenID)
        {
            return await FindByCondition(theoDoi => theoDoi.UserID.ToString() == userID && theoDoi.TruyenID == truyenID)
                    .FirstOrDefaultAsync();
        }

        public async Task<PagedList<Truyen>> GetTruyenByTheoDoiForPagination(TheoDoiParameters theoDoiParameters)
        {
            return await PagedList<Truyen>.ToPagedList(
                (from m in _context.Truyens
                 join n in _context.TheoDois
                 on m.TruyenID equals n.TruyenID
                 join c in _context.Users
                 on n.UserID equals c.UserID
                 where c.UserID.ToString() == theoDoiParameters.UserID && !m.TinhTrang && !c.TinhTrang
                 select m).Include(m => m.Chuongs)
                          .OrderBy(on => on.TruyenID),
                theoDoiParameters.PageNumber,
                theoDoiParameters.PageSize);
        }

        public async Task<PagedList<TheoDoi>> GetTheoDoiLastestForPagination(TheoDoiParameters theoDoiParameters)
        {
            return await PagedList<TheoDoi>.ToPagedList(FindAll()
                .Include(m => m.User)
                .Include(m => m.Truyen)
                .OrderByDescending(on => on.TheoDoiID).Take(theoDoiParameters.PageSize),
                theoDoiParameters.PageNumber,
                theoDoiParameters.PageSize);
        }
    }
}
