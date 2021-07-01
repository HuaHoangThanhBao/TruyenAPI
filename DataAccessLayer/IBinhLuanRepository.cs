using CoreLibrary.Helpers;
using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IBinhLuanRepository
    {
        Task<PagedList<BinhLuan>> GetBinhLuanForPagination(BinhLuanParameters binhLuanParameters);
        Task<PagedList<BinhLuan>> GetBinhLuanLastestForPagination(BinhLuanParameters binhLuanParameters);
        Task<PagedList<BinhLuan>> GetBinhLuanOfTruyenForPagination(int truyenID, BinhLuanParameters binhLuanParameters);
        Task<PagedList<BinhLuan>> GetBinhLuanOfChuongForPagination(int chuongID, BinhLuanParameters binhLuanParameters);

        Task<IEnumerable<BinhLuan>> GetAllBinhLuansAsync();
        Task<BinhLuan> GetBinhLuanByIdAsync(int binhLuanId);
        Task<BinhLuan> GetBinhLuanByDetailAsync(int binhLuanId);
        ResponseDetails CreateBinhLuan(BinhLuan binhLuan);
        ResponseDetails UpdateBinhLuan(BinhLuan binhLuan);
        ResponseDetails DeleteBinhLuan(BinhLuan binhLuan);
    }
}
