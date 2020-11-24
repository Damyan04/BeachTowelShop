using BeachTowelShop.Areas.Admin.Models;
using BeachTowelShop.Models.Products;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Automapper
{
    public class SizeViewModelProfile : AutoMapper.Profile
    {
        public SizeViewModelProfile()
        {
           


            CreateMap<SizeDto, SizeViewModel>()
             .ForMember(s => s.Id, t => t.MapFrom(src => src.Id))
             .ForMember(s => s.Price, t => t.MapFrom(src => src.Price))
             .ForMember(s => s.SizeName, t => t.MapFrom(src => src.Name))
            
             .ReverseMap();


            CreateMap<SizeDto, HomePageTowelsViewModel>()
         
          .ForMember(s => s.Price, t => t.MapFrom(src => src.Price))
          .ForMember(s => s.Size, t => t.MapFrom(src => src.Name))
           .ForMember(s => s.PicturePath, t => t.MapFrom(src => src.SizePicturePath))
          .ReverseMap();


            CreateMap<SizeDto, AdminSizesViewModel>()
             .ForMember(s => s.Id, t => t.MapFrom(src => src.Id))
             .ForMember(s => s.Price, t => t.MapFrom(src => src.Price))
             .ForMember(s => s.Name, t => t.MapFrom(src => src.Name))

             .ReverseMap();

        }
    }
}
