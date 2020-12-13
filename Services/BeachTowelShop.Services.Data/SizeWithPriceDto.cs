using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public  class SizeWithPriceDto
    {
        public string Size { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Price { get; set; }
        public string Id { get; set; }
    }
}
