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
        private ITruyenRepository _truyen;
        private IPhuLucRepository _phuLuc;
        private INoiDungTruyenRepository _noiDungTruyen;
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
        public ITruyenRepository Truyen
        {
            get
            {
                if (_truyen == null)
                {
                    _truyen = new TruyenRepository(_repoContext);
                }
                return _truyen;
            }
        }
        public IPhuLucRepository PhuLuc
        {
            get
            {
                if (_phuLuc == null)
                {
                    _phuLuc = new PhuLucRepository(_repoContext);
                }
                return _phuLuc;
            }
        }
        public INoiDungTruyenRepository NoiDungTruyen
        {
            get
            {
                if (_noiDungTruyen == null)
                {
                    _noiDungTruyen = new NoiDungTruyenRepository(_repoContext);
                }
                return _noiDungTruyen;
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
