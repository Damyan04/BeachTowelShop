using AutoMapper;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeachTowelShop.Services.Automapper
{
  public  class AdminDtoProfile : Profile
    {
        public AdminDtoProfile()
        {
            CreateMap<Product, AdminProductDto>()

         .ForMember(s => s.Description, t => t.MapFrom(src => src.Description))
         .ForMember(s => s.Name, t => t.MapFrom(src => src.Name))
         .ForMember(s => s.OrderCount, t => t.MapFrom(src => src.OrderCount))
          .ForMember(s => s.Id, t => t.MapFrom(src => src.Id))
         
         .ReverseMap();

           

            CreateMap<ProductSize, SizeWithPriceDto>()
                 .ForMember(s => s.Price, t => t.MapFrom(src => src.Price))
                .ForMember(s => s.Size, t => t.MapFrom(src => src.Size.Name))
                 .ForMember(s => s.Id, t => t.MapFrom(src => src.SizeId)).ReverseMap();

            


        }
      
    }
}
