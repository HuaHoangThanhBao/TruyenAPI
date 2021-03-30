using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ITruyenRepository
    {
        Task<IEnumerable<Truyen>> GetAllTruyensAsync();
        Task<Truyen> GetTruyenByIdAsync(int truyenId);
        Task<Truyen> GetTruyenByDetailAsync(int truyenId);
        ResponseDetails CreateTruyen(Truyen truyen);
        ResponseDetails UpdateTruyen(Truyen truyen);
        ResponseDetails DeleteTruyen(Truyen truyen);
    }
}
