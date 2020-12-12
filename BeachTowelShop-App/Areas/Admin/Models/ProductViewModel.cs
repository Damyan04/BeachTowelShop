﻿using BeachTowelShop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop_App.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public List<PictureViewModel> PictureList { get; set; } = new List<PictureViewModel>();

        public int OrderCount { get; set; }
        public string Error { get; set; } = "";
        public List<SizesWithPriceViewModel> SizesPricesList { get; set; } = new List<SizesWithPriceViewModel>();
        public List<CategoryViewModel> CategoryViews { get; set; } = new List<CategoryViewModel>();


    }
}
