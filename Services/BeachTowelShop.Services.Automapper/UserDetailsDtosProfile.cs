using AutoMapper;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Automapper
{
   public class UserDetailsDtosProfile : Profile
    {
        public UserDetailsDtosProfile()
        {
            CreateMap<Order, UserDetailsDto>()
               .ForMember(s => s.Invoice, t => t.MapFrom(src => src.Invoice))
               .ForMember(s => s.InvoiceAdress, t => t.MapFrom(src => src.FirmAdress))
               .ForMember(s => s.InvoiceCity, t => t.MapFrom(src => src.CityFirm))
                 .ForMember(s => s.Adress, t => t.MapFrom(src => src.Adress))
                 .ForMember(s => s.City, t => t.MapFrom(src => src.City))
                 .ForMember(s => s.DeliveryMethod, t => t.MapFrom(src => src.DeliveryMethod))
                 .ForMember(s => s.Email, t => t.MapFrom(src => src.Email))
                  .ForMember(s => s.DeliveryAdress, t => t.MapFrom(src => src.DeliveryAdress))
                 .ForMember(s => s.FullName, t => t.MapFrom(src => src.Name))
                 .ForMember(s => s.InvoiceDDS, t => t.MapFrom(src => src.DDSN))
                  .ForMember(s => s.InvoiceEIK, t => t.MapFrom(src => src.EIK))
                   .ForMember(s => s.InvoiceFirm, t => t.MapFrom(src => src.Firm))
                    .ForMember(s => s.InvoiceMOL, t => t.MapFrom(src => src.MOL))
                    .ForMember(s => s.InvoiceType, t => t.MapFrom(src => src.InvoiceType))
                    .ForMember(s => s.PaymentMethod, t => t.MapFrom(src => src.PaymentMethod))
                     .ForMember(s => s.Phone, t => t.MapFrom(src => src.Phone))
                      .ForMember(s => s.UsersessionId, t => t.MapFrom(src => src.UsersessionId))
                        .ForMember(s => s.Sum, t => t.MapFrom(src => src.Sum))
               .ReverseMap();
        }
    }
}
