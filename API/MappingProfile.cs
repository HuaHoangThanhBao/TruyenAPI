using AutoMapper;
using CoreLibrary.DataTransferObjects;
using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TacGia, TacGiaDto>();

            CreateMap<TheLoai, TheLoaiDto>();

            CreateMap<Truyen, TruyenDto>();

            CreateMap<PhuLuc, PhuLucDto>();

            CreateMap<Chuong, ChuongDto>();

            CreateMap<NoiDungChuong, NoiDungChuongDto>();

            CreateMap<User, UserDto>();

            CreateMap<TheoDoi, TheoDoiDto>();

            CreateMap<BinhLuan, BinhLuanDto>();

            CreateMap<UserForRegistrationDto, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

            //CRUD Map
            CreateMap<TacGiaForCreationDto, TacGia>();

            CreateMap<TacGiaForUpdateDto, TacGia>();

            CreateMap<TheLoaiForCreationDto, TheLoai>();

            CreateMap<TheLoaiForUpdateDto, TheLoai>();

            CreateMap<TruyenForCreationDto, Truyen>();

            CreateMap<TruyenForUpdateDto, Truyen>();

            CreateMap<PhuLucForCreationDto, PhuLuc>();

            CreateMap<PhuLucForUpdateDto, PhuLuc>();

            CreateMap<ChuongForCreationDto, Chuong>();

            CreateMap<ChuongForUpdateDto, Chuong>();
            
            /*Thêm ngày 08/06/2021*/
            CreateMap<NoiDungChuongForCreationDto, NoiDungChuong>();

            CreateMap<NoiDungChuongForUpdateDto, NoiDungChuong>();
            /**/

            CreateMap<UserForCreationDto, User>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<TheoDoiForCreationDto, TheoDoi>();

            CreateMap<TheoDoiForUpdateDto, TheoDoi>();

            CreateMap<BinhLuanForCreationDto, BinhLuan>();

            CreateMap<BinhLuanForUpdateDto, BinhLuan>();
        }
    }
}
