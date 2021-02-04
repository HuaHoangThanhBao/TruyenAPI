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
        bool CreatePhuLuc(IEnumerable<PhuLuc> phuLucs);
        bool UpdatePhuLuc(PhuLuc phuLuc);
        void DeletePhuLuc(PhuLuc phuLuc);
    }
}
