using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITruyenRepository
    {
        Task<IEnumerable<Truyen>> GetAllTruyensAsync();
        Task<Truyen> GetTruyenByIdAsync(int truyenId);
        Task<Truyen> GetTruyenByDetailAsync(int truyenId);
        Truyen CreateTruyen(IEnumerable<Truyen> truyens);
        bool UpdateTruyen(Truyen truyen);
        void DeleteTruyen(Truyen truyen);
    }
}
