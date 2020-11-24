using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Automapper
{
    public class PictureDtosProfile : AutoMapper.Profile

    {
        public PictureDtosProfile()
        {
            CreateMap<Picture, PictureDto>().ReverseMap();
        }
    }
}
