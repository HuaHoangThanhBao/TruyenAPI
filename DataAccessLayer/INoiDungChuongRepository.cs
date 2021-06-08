using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface INoiDungChuongRepository
    {
        Task<IEnumerable<NoiDungChuong>> GetAllNoiDungChuongsAsync();
        Task<NoiDungChuong> GetNoiDungChuongByChuongIdAsync(int chuongId);
        ResponseDetails CreateNoiDungChuong(IEnumerable<NoiDungChuong> noiDungChuong);
        ResponseDetails UpdateNoiDungChuong(NoiDungChuong noiDungChuong);
        ResponseDetails DeleteNoiDungChuong(NoiDungChuong noiDungChuong);
    }
}
