using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ITheoDoiRepository
    {
        Task<IEnumerable<TheoDoi>> GetAllTheoDoisAsync();
        Task<TheoDoi> GetTheoDoiByIdAsync(int theoDoiId);
        ResponseDetails CreateTheoDoi(TheoDoi theoDoi);
        ResponseDetails UpdateTheoDoi(TheoDoi theoDoi);
        ResponseDetails DeleteTheoDoi(TheoDoi theoDoi);
    }
}
