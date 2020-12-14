using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public  class SizeWithPriceDto
    {
        [Required]
        public string Size { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Price { get; set; }
        [Required]
        public string Id { get; set; }
    }
}
