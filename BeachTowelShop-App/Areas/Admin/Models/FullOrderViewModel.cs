using BeachTowelShop.Models.Orders;
using BeachTowelShop_App.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Areas.Admin.Models
{
    public class FullOrderViewModel
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
        public Status Status { get; set; }

        public List<CartViewModel> ListOfProducts { get; set; } = new List<CartViewModel>();
        public List<TextOrderDataViewModel> TextOrderDataViews { get; set; } = new List<TextOrderDataViewModel>();

        //TODO:LIST OF DESIGNITEMS
        //LIST OF TEXTITEMS
    }
}
