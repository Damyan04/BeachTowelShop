using System;
using System.Collections.Generic;

namespace BeachTowelShop.Services.Data
{
    public class ProductDto
    {
        public ProductDto()
        {
            PictureList = new List<string>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> PictureList { get; set; }

        public string Description { get; set; }
       
    }
}
