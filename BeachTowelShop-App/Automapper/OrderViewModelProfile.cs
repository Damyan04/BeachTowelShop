using BeachTowelShop.Areas.Admin.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Automapper
{
    public class OrderViewModelProfile : AutoMapper.Profile
    {
        public OrderViewModelProfile()
        {
            CreateMap<PartialOrderDto, OrderViewModel>()
           .ForMember(s => s.Id, t => t.MapFrom(src => src.Id))
           .ForMember(s => s.Sum, t => t.MapFrom(src => src.Sum))
           .ForMember(s => s.Status, t => t.MapFrom(src => src.Status))

           .ReverseMap();



            CreateMap<FullOrderDto, FullOrderViewModel>()
          .ForMember(s => s.Invoice, t => t.MapFrom(src => src.Invoice))
               .ForMember(s => s.FirmAdress, t => t.MapFrom(src => src.FirmAdress))
               .ForMember(s => s.CityFirm, t => t.MapFrom(src => src.CityFirm))
                 .ForMember(s => s.Adress, t => t.MapFrom(src => src.Adress))
                 .ForMember(s => s.City, t => t.MapFrom(src => src.City))
                 .ForMember(s => s.DeliveryMethod, t => t.MapFrom(src => src.DeliveryMethod))
                 .ForMember(s => s.Email, t => t.MapFrom(src => src.Email))
                  .ForMember(s => s.DeliveryAdress, t => t.MapFrom(src => src.DeliveryAdress))
                 .ForMember(s => s.Name, t => t.MapFrom(src => src.Name))
                 .ForMember(s => s.DDSN, t => t.MapFrom(src => src.DDSN))
                  .ForMember(s => s.EIK, t => t.MapFrom(src => src.EIK))
                   .ForMember(s => s.Firm, t => t.MapFrom(src => src.Firm))
                    .ForMember(s => s.MOL, t => t.MapFrom(src => src.MOL))
                    .ForMember(s => s.InvoiceType, t => t.MapFrom(src => src.InvoiceType))
                    .ForMember(s => s.PaymentMethod, t => t.MapFrom(src => src.PaymentMethod))
                     .ForMember(s => s.Phone, t => t.MapFrom(src => src.Phone))
                      .ForMember(s => s.UsersessionId, t => t.MapFrom(src => src.UsersessionId))
                        .ForMember(s => s.Sum, t => t.MapFrom(src => src.Sum))
                 .ForMember(s => s.Status, t => t.MapFrom(src => src.Status))
                  
           .ReverseMap();
        }
    }
}
