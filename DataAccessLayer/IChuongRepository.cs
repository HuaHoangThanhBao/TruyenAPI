using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IChuongRepository
    {
        Task<IEnumerable<Chuong>> GetAllChuongsAsync();
        Task<IEnumerable<Chuong>> GetAllChuongsByTruyenIdAsync(int truyenID);
        Task<Chuong> GetNewestChuongByTruyenIdAsync(int truyenID);
        Task<Chuong> GetChuongByIdAsync(int chuongId);
        Task<Chuong> GetChuongByDetailAsync(int chuongId);
        ResponseDetails CreateChuong(IEnumerable<Chuong> chuongs);
        ResponseDetails UpdateChuong(Chuong chuong);
        ResponseDetails DeleteChuong(Chuong chuong);
        ResponseDetails DeleteHardChuong(Chuong chuong);
    }
}
