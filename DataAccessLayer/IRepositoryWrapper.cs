namespace DataAccessLayer
{
    public interface IRepositoryWrapper
    {
        ITacGiaRepository TacGia { get; }
        ITheLoaiRepository TheLoai { get; }
        ITruyenRepository Truyen { get; }
        IPhuLucRepository PhuLuc { get; }
        INoiDungChuongRepository NoiDungChuong { get; }
        IChuongRepository Chuong { get; }
        IUserRepository User { get; }
        ITheoDoiRepository TheoDoi { get; }
        IBinhLuanRepository BinhLuan { get; }
        void Save();
    }
}
