using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITacGiaRepository
    {
        Task<IEnumerable<TacGia>> GetAllTacGiasAsync();
        Task<TacGia> GetTacGiaByIdAsync(Guid ownerId);
        Task<TacGia> GetTacGiaByDetailAsync(Guid ownerId);
        TacGia CreateTacGia(IEnumerable<TacGia> owner);
        bool UpdateTacGia(TacGia owner);
        void DeleteTacGia(TacGia owner);
    }
}
