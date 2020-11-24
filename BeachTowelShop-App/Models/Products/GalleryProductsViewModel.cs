using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Products
{
    public class GalleryProductsViewModel
    {
        public GalleryProductsViewModel()
        {
            AllCategories = new List<CategoryViewModel>();
            AllProducts = new List<GalleryProductViewModel>();
        }

        public List<GalleryProductViewModel> AllProducts { get; set; }
        public List<CategoryViewModel> AllCategories { get; set; }
    }
}
