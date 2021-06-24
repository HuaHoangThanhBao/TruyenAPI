using CoreLibrary.Helpers;
using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ITruyenRepository
    {
        PagedList<Truyen> GetTruyenForPagination(TruyenParameters truyenParameters);
        PagedList<Truyen> GetTruyenLastestUpdateForPagination(TruyenParameters truyenParameters);
        PagedList<Truyen> GetTruyenOfTheLoaiForPagination(int theLoaiID, TruyenParameters truyenParameters);
        PagedList<Truyen> GetTruyenOfTheoDoiForPagination(Guid userID, TruyenParameters truyenParameters);
        PagedList<Truyen> GetTopViewForPagination(TruyenParameters truyenParameters);

        Task<IEnumerable<Truyen>> GetAllTruyensAsync();
        Task<Truyen> GetTruyenByIdAsync(int truyenId);
        Task<Truyen> GetTruyenByDetailAsync(int truyenId);
        ResponseDetails CreateTruyen(IEnumerable<Truyen> truyens);
        ResponseDetails UpdateTruyen(Truyen truyen);
        ResponseDetails DeleteTruyen(Truyen truyen);
    }
}
