using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPhuLucRepository
    {
        IEnumerable<PhuLuc> TheLoaisInPhuLuc(int theLoaiId);
        Task<IEnumerable<PhuLuc>> GetAllPhuLucsAsync();
        Task<PhuLuc> GetPhuLucByIdAsync(int phuLucId);
        ResponseDetails CreatePhuLuc(IEnumerable<PhuLuc> phuLucs);
        ResponseDetails UpdatePhuLuc(PhuLuc phuLuc);
        ResponseDetails DeletePhuLuc(PhuLuc phuLuc);
    }
}
