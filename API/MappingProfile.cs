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

            CreateMap<TacGiaForCreationDto, TacGia>();

            CreateMap<TacGiaForUpdateDto, TacGia>();
        }
    }
}
