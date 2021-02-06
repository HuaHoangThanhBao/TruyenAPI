using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface INoiDungTruyenRepository
    {
        Task<IEnumerable<NoiDungTruyen>> GetAllNoiDungTruyensAsync();
        Task<NoiDungTruyen> GetNoiDungTruyenByTruyenIdAsync(int noiDungId, int truyenId);
        ResponseDetails CreateNoiDungTruyen(NoiDungTruyen noiDungTruyen);
        ResponseDetails UpdateNoiDungTruyen(NoiDungTruyen noiDungTruyen);
        ResponseDetails DeleteNoiDungTruyen(int truyenId);
    }
}
