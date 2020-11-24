using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public class GalleryProductDto
    {
        public GalleryProductDto()
        {
            PictureList = new List<string>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> PictureList { get; set; }
    }
}
