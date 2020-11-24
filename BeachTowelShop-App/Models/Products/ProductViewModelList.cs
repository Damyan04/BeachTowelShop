using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop.Models.Products
{
    public class ProductViewModelList
    {
        public ProductViewModelList()
        {
            SimilarProducts = new List<ProductViewModel>();
        }
        public List<ProductViewModel> SimilarProducts { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
