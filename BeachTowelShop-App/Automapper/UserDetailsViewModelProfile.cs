using BeachTowelShop.Models.Orders;
using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Automapper
{
    public class UserDetailsViewModelProfile : AutoMapper.Profile
    {
        public UserDetailsViewModelProfile()
        {
            CreateMap<UserDetailsDto, UserDetailsViewModel>()
            .ForMember(s => s.Adress, t => t.MapFrom(src => src.Adress))
            .ForMember(s => s.City, t => t.MapFrom(src => src.City))
            .ForMember(s => s.DeliveryMethod, t => t.MapFrom(src => src.DeliveryMethod))
            .ForMember(s => s.Email, t => t.MapFrom(src => src.Email))
             .ForMember(s => s.DeliveryMethod, t => t.MapFrom(src => src.DeliveryMethod))
             .ForMember(s => s.FullName, t => t.MapFrom(src => src.FullName))
           .ForMember(s => s.Invoice, t => t.MapFrom(src => src.Invoice))
              .ForMember(s => s.InvoiceAdress, t => t.MapFrom(src => src.InvoiceAdress))
               .ForMember(s => s.InvoiceCity, t => t.MapFrom(src => src.InvoiceCity))
               .ForMember(s => s.InvoiceDDS, t => t.MapFrom(src => src.InvoiceDDS))
               .ForMember(s => s.InvoiceEIK, t => t.MapFrom(src => src.InvoiceEIK))
               .ForMember(s => s.InvoiceFirm, t => t.MapFrom(src => src.InvoiceFirm))
                .ForMember(s => s.InvoiceMOL, t => t.MapFrom(src => src.InvoiceMOL))
                 .ForMember(s => s.InvoiceType, t => t.MapFrom(src => src.InvoiceType))
                  .ForMember(s => s.PaymentMethod, t => t.MapFrom(src => src.PaymentMethod))
                  .ForMember(s => s.Phone, t => t.MapFrom(src => src.Phone))
                  .ForMember(s => s.Sum, t => t.MapFrom(src => src.Sum))
                   .ForMember(s => s.DeliveryAdress, t => t.MapFrom(src => src.DeliveryAdress))
            .ReverseMap();
        }
    }
}
