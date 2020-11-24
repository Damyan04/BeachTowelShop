using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Products
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            PictureList = new List<string>();
            Sizes = new List<SizeViewModel>();
        }
        public string Name { get; set; } 
        public string Id { get; set; }
        public List<string> PictureList { get; set; }

        public  List<SizeViewModel> Sizes{ get; set; }
        public double Price { get; set; } 

        public int Count { get; set; } 
        public string Description { get; set; } 

        //public List<string> Category { get; set; } = new List<string>();
     
    public double Sum { get; set; } 
    }
}
