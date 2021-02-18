using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface INoiDungChuongRepository
    {
        Task<IEnumerable<NoiDungChuong>> GetAllNoiDungChuongsAsync();
        Task<NoiDungChuong> GetNoiDungChuongByChuongIdAsync(int noiDungId, int chuongId);
        ResponseDetails CreateNoiDungChuong(NoiDungChuong noiDungChuong);
        ResponseDetails UpdateNoiDungChuong(NoiDungChuong noiDungChuong);
        ResponseDetails DeleteNoiDungChuong(int chuongId);
    }
}
