using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Products
{
    public class OrderProductViewModel
    {
        public OrderProductViewModel()
        {
            SizeList = new List<SizeViewModel>();
        }
        public List<SizeViewModel> SizeList { get; set; }
        public int Count { get; set; }
       
    }
}
