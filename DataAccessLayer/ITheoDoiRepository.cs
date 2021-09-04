using CoreLibrary.Helpers;
using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ITheoDoiRepository
    {
        Task<PagedList<Truyen>> GetTruyenByTheoDoiForPagination(TheoDoiParameters theoDoiParameters);
        Task<PagedList<TheoDoi>> GetTheoDoiLastestForPagination(TheoDoiParameters theoDoiParameters);
        Task<IEnumerable<TheoDoi>> GetAllTheoDoisAsync();

        Task<IEnumerable<TheoDoi>> GetTheoDoiByUserIdAsync(Guid userId);
        Task<TheoDoi> GetTheoDoiByIdAsync(int theoDoiId);
        Task<TheoDoi> GetTheoDoiByUserIdAndTruyenIdAsync(string userID, int truyenID);
        ResponseDetails CreateTheoDoi(TheoDoi theoDoi);
        ResponseDetails UpdateTheoDoi(TheoDoi theoDoi);
        ResponseDetails DeleteTheoDoi(TheoDoi theoDoi);
    }
}
