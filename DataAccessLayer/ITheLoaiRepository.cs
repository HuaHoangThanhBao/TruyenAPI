using CoreLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
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
