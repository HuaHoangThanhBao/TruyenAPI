﻿using DataAccessLayer;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.Extensions;
using CoreLibrary.Helpers;
using System;
using System.Text;
using System.Text.RegularExpressions;

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
            /*Kiểm tra xem chuỗi json nhập vào có bị trùng tên truyện không*/
            foreach (var dup in truyens.GroupBy(p => p.TenTruyen))
            {
                if(dup.Count() - 1 > 0)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Chuỗi json nhập vào bị trùng tên truyện",
                        Value = dup.Key.ToString()
                    };
                }
            }
            /*End*/

            var tacGiaRepo = new TacGiaRepository(_context);

            foreach (var truyen in truyens)
            {
                /*Bắt lỗi [ID]*/
                if (!tacGiaRepo.FindByCondition(t => t.TacGiaID.Equals(truyen.TacGiaID)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "ID tác giả không tồn tại",
                        Value = truyen.TacGiaID.ToString()
                    };
                }
                /*End*/

                /*Bắt lỗi [Tên truyện]*/
                if (FindByCondition(t => t.TenTruyen.Equals(truyen.TenTruyen)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên truyện bị trùng",
                        Value = truyen.TenTruyen
                    };
                }
                /*End*/

                Create(truyen);
            }

            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTruyen bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateTruyen(Truyen truyen)
        {
            /*Bắt lỗi [ID]*/
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
            /*End*/

            /*Bắt lỗi [Tên truyện]*/
            if (FindByCondition(t => t.TenTruyen.Equals(truyen.TenTruyen) && t.TruyenID != truyen.TruyenID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên truyện bị trùng"
                };
            }
            /*End*/

            //Tạo bản ghi mới nhưng chưa update vào CSDL
            Update(truyen);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa truyện thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteTruyen(Truyen truyen)
        {
            //Tạo bản ghi mới nhưng chưa update vào CSDL
            truyen.TinhTrang = true;
            Update(truyen);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa truyện thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<Truyen>> GetAllTruyensAsync()
        {
            return await FindAll()
                .Where(ow => !ow.TinhTrang)
                .OrderBy(ow => ow.TruyenID)
                .ToListAsync();
        }

        public async Task<Truyen> GetTruyenByIdAsync(int truyenId)
        {
            return await FindByCondition(truyen => truyen.TruyenID.Equals(truyenId) && !truyen.TinhTrang)
                    .FirstOrDefaultAsync();
        }

        public async Task<Truyen> GetTruyenByDetailAsync(int truyenId)
        {
            return await FindByCondition(truyen => truyen.TruyenID.Equals(truyenId) && !truyen.TinhTrang)
                .Include(a => a.TacGia)
                .Include(a => a.Chuongs)
                //    .ThenInclude(a => a.BinhLuans)
                //    .ThenInclude(b => b.User)
                //.Include(a => a.PhuLucs)
                //    .ThenInclude(b => b.TheLoai)
                //.Include(a => a.TheoDois)
                //    .ThenInclude(b => b.User)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedList<Truyen>> GetTruyenForPagination(TruyenParameters truyenParameters)
        {
            return await PagedList<Truyen>.ToPagedList(FindAll().Where(m => !m.TinhTrang).Include(m => m.Chuongs)
                .OrderBy(on => on.TenTruyen),
                truyenParameters.PageNumber,
                truyenParameters.PageSize);
        }

        public async Task<PagedList<Truyen>> GetTruyenLastestUpdateForPagination(TruyenParameters truyenParameters)
        {
            var chuongs = (from m in _context.Chuongs
                           where !m.TinhTrang
                           orderby m.ThoiGianCapNhat descending
                           select m);

            return await PagedList<Truyen>.ToPagedList(

               _context.Truyens.Where(m => !m.TinhTrang).OrderByDescending(user => user.Chuongs.Max(d => d.ThoiGianCapNhat)).Include(m => m.Chuongs)

                ,
                truyenParameters.PageNumber,
                truyenParameters.PageSize);
        }

        public async Task<PagedList<Truyen>> GetTruyenOfTheLoaiForPagination(int theLoaiID, TruyenParameters truyenParameters)
        {
            return await PagedList<Truyen>.ToPagedList(

                (from m in _context.Truyens
                 join n in _context.PhuLucs
                 on m.TruyenID equals n.TruyenID
                 where n.TheLoaiID == theLoaiID && !m.TinhTrang && !n.TinhTrang
                 select m).Include(m => m.Chuongs).Distinct().OrderBy(m => m.TruyenID)

                ,
                truyenParameters.PageNumber,
                truyenParameters.PageSize);
        }

        //public async Task<PagedList<Truyen>> GetTruyenOfTheoDoiForPagination(Guid userID, TruyenParameters truyenParameters)
        //{
        //    return await PagedList<Truyen>.ToPagedList(

        //        (from m in _context.Truyens
        //         join n in _context.TheoDois
        //         on m.TruyenID equals n.TruyenID
        //         where n.UserID == userID && !m.TinhTrang
        //         select m).Include(m => m.Chuongs).Distinct().OrderBy(m => m.TruyenID)

        //        ,
        //        truyenParameters.PageNumber,
        //        truyenParameters.PageSize);
        //}

        public async Task<PagedList<Truyen>> GetTopViewForPagination(TruyenParameters truyenParameters)
        {
            return await PagedList<Truyen>.ToPagedList(

               _context.Truyens.Where(m => !m.TinhTrang).OrderByDescending(user => user.Chuongs.Sum(d => d.LuotXem))
               .Take(truyenParameters.PageSize).Include(m => m.Chuongs)

                ,
                truyenParameters.PageNumber,
                truyenParameters.PageSize);
        }

        public async Task<IEnumerable<Truyen>> FindTruyenForPagination()
        {
            return await
                FindAll()
                .Where(ow => !ow.TinhTrang)
                .OrderBy(on => on.TruyenID)
                .ToListAsync();
        }
    }
}
