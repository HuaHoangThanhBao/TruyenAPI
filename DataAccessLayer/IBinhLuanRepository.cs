using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IBinhLuanRepository
    {
        Task<IEnumerable<BinhLuan>> GetAllBinhLuansAsync();
        Task<BinhLuan> GetBinhLuanByIdAsync(int binhLuanId);
        Task<BinhLuan> GetBinhLuanByDetailAsync(int binhLuanId);
        ResponseDetails CreateBinhLuan(BinhLuan binhLuan);
        ResponseDetails UpdateBinhLuan(BinhLuan binhLuan);
        ResponseDetails DeleteBinhLuan(BinhLuan binhLuan);
    }
}
