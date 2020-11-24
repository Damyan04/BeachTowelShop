using AutoMapper;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeachTowelShop.Services.Automapper
{
   public class CategoryDtosProfile : Profile
    {
        public CategoryDtosProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            // .ForMember(id=>id.Id,opt=>opt.MapFrom(src=>src.Id))
                //.ForMember(name => name.Name, opt => opt.MapFrom(src => src.Name))
            //CreateMap<List<Category>, List<CategoryDto>>();
            //CreateMap<CategoryDto, CategoryViewModel>().ReverseMap();
        }
    }
}
