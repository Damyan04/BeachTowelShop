using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeachTowelShop.Data.Models
{
   public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
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
        //maybe separete entity like ProductOrder that only has the fields we need
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<TextProperty> TextProperties { get; set; } = new List<TextProperty>();
      
    }
}
