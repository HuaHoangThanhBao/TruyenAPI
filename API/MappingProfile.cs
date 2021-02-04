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

            CreateMap<TacGiaForCreationDto, TacGia>();

            CreateMap<TacGiaForUpdateDto, TacGia>();

            CreateMap<TheLoaiForCreationDto, TheLoai>();

            CreateMap<TheLoaiForUpdateDto, TheLoai>();

            CreateMap<TruyenForCreationDto, Truyen>();

            CreateMap<TruyenForUpdateDto, Truyen>();

            CreateMap<PhuLucForCreationDto, PhuLuc>();

            CreateMap<PhuLucForUpdateDto, PhuLuc>();
        }
    }
}
