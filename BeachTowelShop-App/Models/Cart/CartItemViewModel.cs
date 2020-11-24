using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Cart
{
    public class CartItemViewModel
    {
        public string ProductId { get; set; }
        public string SessionName { get; set; }
        public string ImgName { get; set; }

        public string Count { get; set; }
        public string Size { get; set; }
        public double Sum { get; set; }
    }
}