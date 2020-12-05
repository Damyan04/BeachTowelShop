using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Areas.Admin.Models
{
    public class OrderViewModel
    {
        public string Id { get; set; }

        public double Sum { get; set; }

        public string Status { get; set; }
    }
}
