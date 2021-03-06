﻿using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ITacGiaRepository
    {
        Task<IEnumerable<TacGia>> GetAllTacGiasAsync();
        Task<TacGia> GetTacGiaByIdAsync(int tacGiaId);
        Task<TacGia> GetTacGiaByDetailAsync(int tacGiaId);
        ResponseDetails CreateTacGia(IEnumerable<TacGia> tacGias);
        ResponseDetails UpdateTacGia(TacGia tacGia);
        ResponseDetails DeleteTacGia(TacGia tacGia);
    }
}
