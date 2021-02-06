namespace DataAccessLayer
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
