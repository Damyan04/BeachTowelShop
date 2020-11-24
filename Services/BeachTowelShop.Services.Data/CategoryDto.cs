using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
 public   class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<string> ProductId { get; set; }
    }
}
