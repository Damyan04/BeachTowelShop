using BeachTowelShop.Services.Data;
using BeachTowelShop_App.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop_App.Automapper
{
    public class AdminViewModelProfile : AutoMapper.Profile
    {
        public AdminViewModelProfile()
        {
            CreateMap<AdminProductDto,ProductViewModel>()
         .ReverseMap();

            CreateMap<SizeWithPriceDto, SizesWithPriceViewModel>()
        .ReverseMap();
            CreateMap<PictureDto, PictureViewModel>()
        .ReverseMap();

            
        }
    }
}
