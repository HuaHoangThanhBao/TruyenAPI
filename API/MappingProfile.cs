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

            CreateMap<NoiDungTruyen, NoiDungTruyenDto>();

            CreateMap<User, UserDto>();

            CreateMap<TheoDoi, TheoDoiDto>();

            CreateMap<BinhLuan, BinhLuanDto>();

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

            CreateMap<UserForCreationDto, User>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<TheoDoiForCreationDto, TheoDoi>();

            CreateMap<TheoDoiForUpdateDto, TheoDoi>();

            CreateMap<BinhLuanForCreationDto, BinhLuan>();

            CreateMap<BinhLuanForUpdateDto, BinhLuan>();
        }
    }
}
