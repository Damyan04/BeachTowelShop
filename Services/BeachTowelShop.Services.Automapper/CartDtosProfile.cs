using AutoMapper;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Automapper
{
  public class CartDtosProfile : Profile
    {
        public CartDtosProfile()
        {
            CreateMap<CartItem, UserSessionCartDto>()

           .ForMember(s => s.UserSessionId, t => t.MapFrom(src => src.UserSessionId))
           .ForMember(s => s.Price, t => t.MapFrom(src => src.Price))
           .ForMember(s => s.Size, t => t.MapFrom(src => src.Size))
           .ForMember(s => s.ProductId, t => t.MapFrom(src => src.ProductId))
            .ForMember(s => s.DesignName, t => t.MapFrom(src => src.DesignName))
            .ForMember(s => s.DesignFolderPath, t => t.MapFrom(src => src.DesignFolderPath))
             .ForMember(s => s.Count, t => t.MapFrom(src => src.Count))
             .ForMember(s => s.Sum, t => t.MapFrom(src => src.Sum))
           .ReverseMap();



            CreateMap<TextProperty, UserTextSessionDto>()

           .ForMember(s => s.UserSessionId, t => t.MapFrom(src => src.UserSessionId))
           .ForMember(s => s.Underline, t => t.MapFrom(src => src.Underline))
           .ForMember(s => s.BackgroundColor, t => t.MapFrom(src => src.BackgroundColor))
           .ForMember(s => s.Fill, t => t.MapFrom(src => src.Fill))
             .ForMember(s => s.FontStyle, t => t.MapFrom(src => src.FontStyle))
            .ForMember(s => s.FontFamily, t => t.MapFrom(src => src.FontFamily))
            .ForMember(s => s.FontSize, t => t.MapFrom(src => src.FontSize))
             .ForMember(s => s.FontWeight, t => t.MapFrom(src => src.FontWeight))
              .ForMember(s => s.Overline, t => t.MapFrom(src => src.Overline))
               .ForMember(s => s.Stroke, t => t.MapFrom(src => src.Stroke))
                .ForMember(s => s.Text, t => t.MapFrom(src => src.Text))
           .ReverseMap();
        }
    }
}
