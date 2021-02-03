using Contracts;
using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private ITacGiaRepository _tacGia;
        private ITheLoaiRepository _theLoai;
        public ITacGiaRepository TacGia
        {
            get
            {
                if (_tacGia == null)
                {
                    _tacGia = new TacGiaRepository(_repoContext);
                }
                return _tacGia;
            }
        }
        public ITheLoaiRepository TheLoai
        {
            get
            {
                if (_theLoai == null)
                {
                    _theLoai = new TheLoaiRepository(_repoContext);
                }
                return _theLoai;
            }
        }
        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
