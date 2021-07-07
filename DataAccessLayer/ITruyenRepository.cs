using CoreLibrary.Helpers;
using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ITruyenRepository
    {
        Task<PagedList<Truyen>> GetTruyenForPagination(TruyenParameters truyenParameters);
        Task<PagedList<Truyen>> GetTruyenLastestUpdateForPagination(TruyenParameters truyenParameters);
        Task<PagedList<Truyen>> GetTruyenOfTheLoaiForPagination(int theLoaiID, TruyenParameters truyenParameters);
        //Task<PagedList<Truyen>> GetTruyenOfTheoDoiForPagination(Guid userID, TruyenParameters truyenParameters);
        Task<PagedList<Truyen>> GetTopViewForPagination(TruyenParameters truyenParameters);
        Task<IEnumerable<Truyen>> FindTruyenForPagination();

        Task<IEnumerable<Truyen>> GetAllTruyensAsync();
        Task<Truyen> GetTruyenByIdAsync(int truyenId);
        Task<Truyen> GetTruyenByDetailAsync(int truyenId);
        ResponseDetails CreateTruyen(IEnumerable<Truyen> truyens);
        ResponseDetails UpdateTruyen(Truyen truyen);
        ResponseDetails DeleteTruyen(Truyen truyen);
    }
}
