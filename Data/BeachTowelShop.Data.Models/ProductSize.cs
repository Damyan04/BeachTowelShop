using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeachTowelShop.Data.Models
{
   public class ProductSize
    {
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public string SizeId { get; set; }
        public Size Size { get; set; }

        [Required]
        [Range(0.00, double.MaxValue)]
        public double Price { get; set; }
    }
}
