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
        ResponseDetails CreateTheLoai(IEnumerable<TheLoai> theLoais);
        ResponseDetails UpdateTheLoai(TheLoai theLoai);
        ResponseDetails DeleteTheLoai(TheLoai theLoai);
    }
}
