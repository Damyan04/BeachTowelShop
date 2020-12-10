using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Products
{
    public class GalleryProductViewModel
    {
        public GalleryProductViewModel()
        {
            PictureList = new List<string>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> PictureList { get; set; }
        public  double LowestPrice { get; set; }
        public double HighPrice { get; set; }
    }
}
