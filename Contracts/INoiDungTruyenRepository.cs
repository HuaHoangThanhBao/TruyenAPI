using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface INoiDungTruyenRepository
    {
        Task<IEnumerable<NoiDungTruyen>> GetAllNoiDungTruyensAsync();
        Task<NoiDungTruyen> GetNoiDungTruyenByTruyenIdAsync(int noiDungId, int truyenId);
        ResponseDetails CreateNoiDungTruyen(NoiDungTruyen noiDungTruyen);
        ResponseDetails UpdateNoiDungTruyen(NoiDungTruyen noiDungTruyen);
        ResponseDetails DeleteNoiDungTruyen(int truyenId);
    }
}
