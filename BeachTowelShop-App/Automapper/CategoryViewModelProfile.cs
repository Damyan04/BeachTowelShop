
using BeachTowelShop.Models.Products;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Automapper
{
    public class CategoryViewModelProfile: AutoMapper.Profile
    {
        public CategoryViewModelProfile()
        {
            CreateMap<CategoryDto, CategoryViewModel>();
        }
    }
}
