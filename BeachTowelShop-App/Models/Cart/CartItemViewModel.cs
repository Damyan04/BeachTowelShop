using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Cart
{
    public class CartItemViewModel
    {
        public string ProductId { get; set; }
        public string SessionName { get; set; }
        public string ImgName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public string Count { get; set; }
        public string Size { get; set; }
        public double Sum { get; set; }
    }
}