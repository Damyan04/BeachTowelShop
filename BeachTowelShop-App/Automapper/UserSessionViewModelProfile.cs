using BeachTowelShop.Models.Cart;
using BeachTowelShop.Models.Orders;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Automapper
{
    public class UserSessionViewModelProfile : AutoMapper.Profile
    {
        public UserSessionViewModelProfile()
        {
            CreateMap<UserSessionCartDto, OrderDataViewModel>()
            .ForMember(s => s.SessionId, t => t.MapFrom(src => src.UserSessionId))
            .ForMember(s => s.Price, t => t.MapFrom(src => src.Sum/src.Count))
            .ForMember(s => s.Size, t => t.MapFrom(src => src.Size))
            .ForMember(s => s.ProductId, t => t.MapFrom(src => src.ProductId))
             .ForMember(s => s.DesignName, t => t.MapFrom(src => src.DesignName))
             .ForMember(s => s.DesignFolderPath, t => t.MapFrom(src => src.DesignFolderPath))
              .ForMember(s => s.Count, t => t.MapFrom(src => src.Count))
              .ForMember(s => s.Sum, t => t.MapFrom(src => src.Sum))
            .ReverseMap();

            //CartViewModel
            CreateMap<UserSessionCartDto, CartViewModel>()
           .ForMember(s => s.SessionId, t => t.MapFrom(src => src.UserSessionId))
           .ForMember(s => s.Price, t => t.MapFrom(src => src.Sum/src.Count))
           .ForMember(s => s.Size, t => t.MapFrom(src => src.Size))
           .ForMember(s => s.ProductId, t => t.MapFrom(src => src.ProductId))
            .ForMember(s => s.DesignName, t => t.MapFrom(src => src.DesignName))
            .ForMember(s => s.DesignFolderPath, t => t.MapFrom(src => src.DesignFolderPath))
             .ForMember(s => s.Count, t => t.MapFrom(src => src.Count))
             .ForMember(s => s.Sum, t => t.MapFrom(src => src.Sum))
           .ReverseMap();
            // CartItem

            CreateMap<UserSessionCartDto, CartItemViewModel>()
           .ForMember(s => s.SessionName, t => t.MapFrom(src => src.UserSessionId))
          
           .ForMember(s => s.Size, t => t.MapFrom(src => src.Size))
           .ForMember(s => s.ProductId, t => t.MapFrom(src => src.ProductId))
            .ForMember(s => s.ImgName, t => t.MapFrom(src => src.DesignName))
          .ForMember(s => s.Sum, t => t.MapFrom(src => src.Sum))
             .ForMember(s => s.Count, t => t.MapFrom(src => src.Count))
             
           .ReverseMap();
        }

    }
}
