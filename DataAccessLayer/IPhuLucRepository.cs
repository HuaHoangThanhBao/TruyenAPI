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
        Task<IEnumerable<PhuLuc>> GetPhuLucByTruyenIdAsync(int truyenId);
        ResponseDetails CreatePhuLuc(IEnumerable<PhuLuc> phuLucs);
        ResponseDetails UpdatePhuLuc(IEnumerable<PhuLuc> phuLucs);
        ResponseDetails DeletePhuLuc(PhuLuc phuLuc);
    }
}
