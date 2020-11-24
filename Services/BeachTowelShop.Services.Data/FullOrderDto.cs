using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
  public  class FullOrderDto
    {

        public string Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string Adress { get; set; }
 
        public string Phone { get; set; }
     
        public string City { get; set; }
        public string Firm { get; set; }
        public string EIK { get; set; }
        public string DDSN { get; set; }
        public string FirmAdress { get; set; }
        public string MOL { get; set; }
        public string CityFirm { get; set; }
        public string InvoiceType { get; set; }
        public string UsersessionId { get; set; }
        public string DeliveryAdress { get; set; }
        public string PaymentMethod { get; set; }
        public bool Invoice { get; set; }
        public string DeliveryMethod { get; set; }
        public double Sum { get; set; }
        public string Status { get; set; }
        public List<UserSessionCartDto> FinishedDesings { get; set; } = new List<UserSessionCartDto>();
        public List<UserTextSessionDto> TextForDesings { get; set; } = new List<UserTextSessionDto>();


    }
}
