using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public class UserDetailsDto
    {
       
        public string FullName { get; set; }
        public string Email { get; set; }
     
        public string Adress { get; set; }
      
        public string City { get; set; }
      
        public string Phone { get; set; }
        public string DeliveryMethod { get; set; } //make it an enum
        public string PaymentMethod { get; set; } //make it an enum
        public bool Invoice { get; set; }
      
        public string InvoiceFirm { get; set; }
       
        public string InvoiceEIK { get; set; }
       
        public string InvoiceAdress { get; set; }
     
        public string InvoiceDDS { get; set; }
        public double Sum { get; set; }
        public string InvoiceMOL { get; set; }
        public string UsersessionId { get; set; }
        public string InvoiceCity { get; set; }
        public string InvoiceType { get; set; }
        public string DeliveryAdress { get; set; }

    }
}
