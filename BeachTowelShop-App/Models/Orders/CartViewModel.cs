using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Orders
{
    public class CartViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public string Count { get; set; }
        public string Size { get; set; }
        public string DesignFolderPath { get; set; }
        public string DesignName { get; set; }
        public double Price { get; set; }
        public string ProductId { get; set; }
        public double Sum { get; set; }
        public string SessionId { get; set; }
    }
}
