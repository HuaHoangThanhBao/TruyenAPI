using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITheLoaiRepository : IRepositoryBase<TheLoai>
    {
        Task<IEnumerable<TheLoai>> GetAllTheLoaisAsync();
        Task<TheLoai> GetTheLoaiByIdAsync(int theLoaiId);
        Task<TheLoai> GetTheLoaiByDetailAsync(int theLoaiId);
        TheLoai CreateTheLoai(IEnumerable<TheLoai> theLoais);
        bool UpdateTheLoai(TheLoai theLoai);
        void DeleteTheLoai(TheLoai theLoai);
    }
}
