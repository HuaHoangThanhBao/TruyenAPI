using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
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
