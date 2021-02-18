using DataAccessLayer;
using CoreLibrary;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private ITacGiaRepository _tacGia;
        private ITheLoaiRepository _theLoai;
        private ITruyenRepository _truyen;
        private IPhuLucRepository _phuLuc;
        private INoiDungChuongRepository _noiDungChuong;
        private IChuongRepository _chuong;
        private IUserRepository _user;
        private ITheoDoiRepository _theoDoi;
        private IBinhLuanRepository _binhLuan;
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
        public INoiDungChuongRepository NoiDungChuong
        {
            get
            {
                if (_noiDungChuong == null)
                {
                    _noiDungChuong = new NoiDungChuongRepository(_repoContext);
                }
                return _noiDungChuong;
            }
        }
        public IChuongRepository Chuong
        {
            get
            {
                if (_chuong == null)
                {
                    _chuong = new ChuongRepository(_repoContext);
                }
                return _chuong;
            }
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }
        public ITheoDoiRepository TheoDoi
        {
            get
            {
                if (_theoDoi == null)
                {
                    _theoDoi = new TheoDoiRepository(_repoContext);
                }
                return _theoDoi;
            }
        }
        public IBinhLuanRepository BinhLuan
        {
            get
            {
                if (_binhLuan == null)
                {
                    _binhLuan = new BinhLuanRepository(_repoContext);
                }
                return _binhLuan;
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
