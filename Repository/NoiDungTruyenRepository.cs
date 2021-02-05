using Contracts;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class NoiDungTruyenRepository : RepositoryBase<NoiDungTruyen>, INoiDungTruyenRepository
    {
        public NoiDungTruyenRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: false = TruyenID hoặc TheLoaiID không tồn tại, true: thêm thành công
        public ResponseDetails CreateNoiDungTruyen(NoiDungTruyen nd)
        {
            if (FindByCondition(t => t.HinhAnh == nd.HinhAnh).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Hình ảnh truyện này đã tồn tại",
                    Value = nd.HinhAnh
                };
            }

            Create(nd);

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Thêm nội dung truyện thành công", Value = nd.HinhAnh };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateNoiDungTruyen(NoiDungTruyen nd)
        {
            Update(nd);

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa nội dung truyện thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteNoiDungTruyen(int truyenId)
        {
            var truyenVsNoiDungTruyenRepo = FindByCondition(t => t.TruyenID.Equals(truyenId));

            foreach (var noiDung in truyenVsNoiDungTruyenRepo)
            {
                Delete(noiDung);
            }

            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa nội dung truyện thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<NoiDungTruyen>> GetAllNoiDungTruyensAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.TruyenID)
                .ToListAsync();
        }

        public async Task<NoiDungTruyen> GetNoiDungTruyenByTruyenIdAsync(int noiDungId, int truyenId)
        {
            return await FindByCondition(truyen => truyen.TruyenID.Equals(truyenId) && truyen.NoiDungTruyenID.Equals(noiDungId))
                .FirstOrDefaultAsync();
        }
    }
}
