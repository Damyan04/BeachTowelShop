using AutoMapper;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Automapper
{
  public  class CommentDtosProfile : Profile
    {
        public CommentDtosProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(s => s.Id, t => t.MapFrom(src => src.Id))
               .ForMember(s=>s.Description,t=>t.MapFrom(src=>src.Description))
                 .ForMember(s => s.ProductId, t => t.MapFrom(src => src.ProductId))
               .ReverseMap();
        }
    }
}
