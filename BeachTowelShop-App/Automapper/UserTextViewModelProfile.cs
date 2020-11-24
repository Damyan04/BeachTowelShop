using BeachTowelShop.Models.Orders;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Automapper
{
    public class UserTextViewModelProfile : AutoMapper.Profile
    {
        public UserTextViewModelProfile()
        {
            CreateMap<UserTextSessionDto, TextOrderDataViewModel>()
           .ForMember(s => s.UserSessionId, t => t.MapFrom(src => src.UserSessionId))
           .ForMember(s => s.Underline, t => t.MapFrom(src => src.Underline))
           .ForMember(s => s.BackgroundColor, t => t.MapFrom(src => src.BackgroundColor))
           .ForMember(s => s.Fill, t => t.MapFrom(src => src.Fill))
            .ForMember(s => s.FontFamily, t => t.MapFrom(src => src.FontFamily))
            .ForMember(s => s.FontSize, t => t.MapFrom(src => src.FontSize))
             .ForMember(s => s.FontWeight, t => t.MapFrom(src => src.FontWeight))
              .ForMember(s => s.Overline, t => t.MapFrom(src => src.Overline))
               .ForMember(s => s.FontStyle, t => t.MapFrom(src => src.FontStyle))
               .ForMember(s => s.Stroke, t => t.MapFrom(src => src.Stroke))
                .ForMember(s => s.Text, t => t.MapFrom(src => src.Text))
           .ReverseMap();
        }
    }
}
