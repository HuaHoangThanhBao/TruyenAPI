using DataAccessLayer;
using CoreLibrary;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.Extensions;

namespace Repository
{
    public class TheLoaiRepository : RepositoryBase<TheLoai>, ITheLoaiRepository
    {
        public TheLoaiRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        //Kiểm tra collection truyền vào có tên trùng trong database không
        //KQ: !null = TenTacGia bị trùng, null: thêm thành công
        public ResponseDetails CreateTheLoai(IEnumerable<TheLoai> theLoais)
        {
            /*Kiểm tra xem chuỗi json nhập vào có bị trùng tên thể loại không*/
            foreach (var dup in theLoais.GroupBy(p => p.TenTheLoai))
            {
                if (dup.Count() - 1 > 0)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Chuỗi json nhập vào bị trùng tên thể loại",
                        Value = dup.Key.ToString()
                    };
                }
            }
            /*End*/

            foreach (var theLoai in theLoais)
            {
                /*Bắt lỗi ký tự đặc biệt*/
                if (ValidationExtensions.isSpecialChar(theLoai.TenTheLoai))
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Không được chứa ký tự đặc biệt",
                        Value = theLoai.TenTheLoai.ToString()
                    };
                }
                /*End*/

                /*Bắt lỗi [Tên thể loại]*/
                if (theLoai.TenTheLoai == "" || theLoai.TenTheLoai == null)
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên thể loại không được để trống",
                        Value = theLoai.TenTheLoai
                    };
                }

                if (FindByCondition(t => t.TenTheLoai.Equals(theLoai.TenTheLoai)).Any())
                {
                    return new ResponseDetails()
                    {
                        StatusCode = ResponseCode.Error,
                        Message = "Tên thể loại bị trùng",
                        Value = theLoai.TenTheLoai
                    };
                }
                /*End*/

                Create(theLoai);
            }
            return new ResponseDetails() { StatusCode = ResponseCode.Success };
        }

        //Kiểm tra object truyền vào có tên trùng trong database không
        //KQ: false: TenTacGia bị trùng, true: cập nhật thành công
        public ResponseDetails UpdateTheLoai(TheLoai theLoai)
        {
            /*Bắt lỗi ký tự đặc biệt*/
            if (ValidationExtensions.isSpecialChar(theLoai.TenTheLoai))
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Không được chứa ký tự đặc biệt",
                    Value = theLoai.TenTheLoai.ToString()
                };
            }
            /*End*/

            /*Bắt lỗi [Tên thể loại]*/
            if (theLoai.TenTheLoai == "" || theLoai.TenTheLoai == null)
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên thể loại không được để trống",
                    Value = theLoai.TenTheLoai
                };
            }

            if (FindByCondition(t => t.TenTheLoai.Equals(theLoai.TenTheLoai) && t.TheLoaiID != theLoai.TheLoaiID).Any())
            {
                return new ResponseDetails()
                {
                    StatusCode = ResponseCode.Error,
                    Message = "Tên thể loại bị trùng",
                    Value = theLoai.TenTheLoai
                };
            }
            /*End*/

            Update(theLoai);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Sửa thể loại thành công" };
        }

        //Xóa logic
        public ResponseDetails DeleteTheLoai(TheLoai theLoai)
        {
            theLoai.TinhTrang = true;
            Update(theLoai);
            return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "Xóa thể loại thành công" };
        }

        //Lấy danh sách các tác giả không bị xóa
        public async Task<IEnumerable<TheLoai>> GetAllTheLoaisAsync()
        {
            return await FindAll()
                .Where(ow => !ow.TinhTrang)
                .OrderBy(ow => ow.TenTheLoai)
                .ToListAsync();
        }

        public async Task<TheLoai> GetTheLoaiByIdAsync(int theLoaiId)
        {
            return await FindByCondition(theLoai => theLoai.TheLoaiID.Equals(theLoaiId))
                    .FirstOrDefaultAsync();
        }

        public async Task<TheLoai> GetTheLoaiByDetailAsync(int theLoaiId)
        {
            return await FindByCondition(theLoai => theLoai.TheLoaiID.Equals(theLoaiId))
                .Include(ac => ac.PhuLucs)
                .FirstOrDefaultAsync();
        }
    }
}
