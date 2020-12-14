using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeachTowelShop.Services.Data
{
 public class CategoryDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<string> ProductId { get; set; }
    }
}
