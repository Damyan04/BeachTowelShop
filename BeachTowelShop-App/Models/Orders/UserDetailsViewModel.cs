﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Orders
{
    public class UserDetailsViewModel
    {

        public List<CartViewModel> CartViewModelList { get; set; } = new List<CartViewModel>();

        [Required]
        public string FullName { get; set; } 
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Adress { get; set; } 
        [Required]
        public string City { get; set; } 
        [Required]
        [Phone]
        [MinLength(8)]
        public string Phone { get; set; }
        [EnumDataType(typeof(DeliveryMethod))]
        public DeliveryMethod DeliveryMethod { get; set;  } //make it an enum
        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; } //make it an enum

        [DefaultValue(false)]
        public bool Invoice { get; set; }
      
        public double Sum { get; set; } 
        public string  InvoiceFirm{ get; set; } 
        
        public string InvoiceEIK { get; set; } 
      
        public string InvoiceAdress { get; set; } 
      
        public string InvoiceDDS { get; set; } 
      
        public string InvoiceMOL { get; set; } 
      
        public string InvoiceCity { get; set; } 
        public string InvoiceType { get; set; }
        public string DeliveryAdress { get; set; }
       

    }
}
