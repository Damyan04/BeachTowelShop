using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Orders
{
    public class OrderDataViewModel
    {
        public string SessionId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Count { get; set; }
        public string Size { get; set; }
        public string DesignFolderPath { get; set; }
        public string DesignName { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Price { get; set; }
        public string ProductId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Sum { get; set; }
    }
}
