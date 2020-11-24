using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;

namespace BeachTowelShop.Services.Automapper
{
    public class ProductDtosProfile: AutoMapper.Profile
    {
        public ProductDtosProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, GalleryProductDto>().ConvertUsing(src => new GalleryProductDto { Id = src.Id, Name = src.Name });
           

        }
    }
}
