using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public class SizeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

       
        public string Pixels { get; set; }
        public double Price { get; set; }
        public string SizePicturePath { get; set; }
    }
}
