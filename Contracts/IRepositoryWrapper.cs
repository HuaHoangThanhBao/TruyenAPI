using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        ITacGiaRepository TacGia { get; }
        ITheLoaiRepository TheLoai { get; }
        ITruyenRepository Truyen { get; }
        IPhuLucRepository PhuLuc { get; }
        INoiDungTruyenRepository NoiDungTruyen { get; }
        void Save();
    }
}
