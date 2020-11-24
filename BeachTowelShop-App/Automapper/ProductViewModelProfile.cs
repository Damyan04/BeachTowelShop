using BeachTowelShop.Models.Products;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Automapper
{
    public class ProductViewModelProfile : AutoMapper.Profile
    {
        public ProductViewModelProfile()
        {
            CreateMap<ProductDto, ProductViewModel>();
            CreateMap<GalleryProductDto, GalleryProductViewModel>();
           
        }
    }
}
