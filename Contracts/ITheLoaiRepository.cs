using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITheLoaiRepository : IRepositoryBase<TheLoai>
    {
        //IEnumerable<TheLoai> AccountsByOwner(int ownerId);
        Task<IEnumerable<TheLoai>> GetAllTheLoaisAsync();
        Task<TheLoai> GetTheLoaiByIdAsync(Guid theLoaiId);
        Task<TheLoai> GetTheLoaiByDetailAsync(Guid theLoaiId);
        TheLoai CreateTheLoai(IEnumerable<TheLoai> theLoais);
        bool UpdateTheLoai(TheLoai theLoai);
        void DeleteTheLoai(TheLoai theLoai);
    }
}
