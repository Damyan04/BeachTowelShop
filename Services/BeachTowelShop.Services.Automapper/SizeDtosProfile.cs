using AutoMapper;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;

using System.Text;

namespace BeachTowelShop.Services.Automapper
{
   public class SizeDtosProfile : Profile
    {
        public SizeDtosProfile()
        {
            CreateMap<Size, SizeDto>()
                 .ForMember(s => s.Id, t => t.MapFrom(src => src.Id))
                .ForMember(s => s.Price, t => t.MapFrom(src => src.Price))
                .ForMember(s => s.Name, t => t.MapFrom(src => src.Name))
                 .ForMember(s=>s.SizePicturePath,t=>t.MapFrom(src=>src.SizePicturePath))
                .ReverseMap();
            CreateMap<ProductSize, SizeDto>()
                .ForMember(s=>s.Id,t=>t.MapFrom(src=>src.SizeId))
                .ForMember(s=>s.Price,t=>t.MapFrom(src=>src.Price))
                .ForMember(s=>s.Name,t=>t.MapFrom(src=>src.Size.Name))
               // .ForMember(s=>s.SizePicturePath,t=>t.MapFrom(src=>src.Size.SizePicturePath))
                .ReverseMap();


        }
    }
}
