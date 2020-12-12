using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
  public  class AdminProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public List<PictureDto> PictureList { get; set; } = new List<PictureDto>();

        public int OrderCount { get; set; }

        public List<SizeWithPriceDto> SizesPricesList { get; set; } = new List<SizeWithPriceDto>();
        public List<CategoryDto> CategoryViews { get; set; } = new List<CategoryDto>();
    }
}
