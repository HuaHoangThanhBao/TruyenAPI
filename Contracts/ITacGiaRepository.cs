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
        Task<TacGia> GetTacGiaByIdAsync(Guid tacGiaId);
        Task<TacGia> GetTacGiaByDetailAsync(Guid tacGiaId);
        TacGia CreateTacGia(IEnumerable<TacGia> tacGias);
        bool UpdateTacGia(TacGia tacGia);
        void DeleteTacGia(TacGia tacGia);
    }
}
