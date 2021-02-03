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
        void Save();
    }
}
