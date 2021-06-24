using CoreLibrary.Helpers;
using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IBinhLuanRepository
    {
        PagedList<BinhLuan> GetBinhLuanForPagination(BinhLuanParameters binhLuanParameters);
        PagedList<BinhLuan> GetBinhLuanLastestForPagination(BinhLuanParameters binhLuanParameters);
        PagedList<BinhLuan> GetBinhLuanOfTruyenForPagination(int truyenID, BinhLuanParameters binhLuanParameters);
        PagedList<BinhLuan> GetBinhLuanOfChuongForPagination(int chuongID, BinhLuanParameters binhLuanParameters);

        Task<IEnumerable<BinhLuan>> GetAllBinhLuansAsync();
        Task<BinhLuan> GetBinhLuanByIdAsync(int binhLuanId);
        Task<BinhLuan> GetBinhLuanByDetailAsync(int binhLuanId);
        ResponseDetails CreateBinhLuan(BinhLuan binhLuan);
        ResponseDetails UpdateBinhLuan(BinhLuan binhLuan);
        ResponseDetails DeleteBinhLuan(BinhLuan binhLuan);
    }
}
