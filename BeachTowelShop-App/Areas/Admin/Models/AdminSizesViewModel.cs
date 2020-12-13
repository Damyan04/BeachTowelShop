using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Areas.Admin.Models
{
    public class AdminSizesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Price { get; set; }
    }
}
