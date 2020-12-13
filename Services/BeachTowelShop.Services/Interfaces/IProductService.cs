using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Interfaces
{
   public interface IProductService
    {
       ICollection<CategoryDto> GetAllCategories();
        ICollection<GalleryProductDto> GetAllProducts();

        ICollection<GalleryProductDto> GetAllProductsForCategory(string categoryId);

        ProductDto GetProductById(string itemid);

        ICollection<ProductDto> GetSimilarProductsById(string itemid);
        ICollection<SizeDto> GetAllSizesForProductById(string itemid);

        ICollection<SizeDto> GetAllSizes();

        double GetPriceForSize(string size, string productId);
        bool VerifyId(string productId);
        ICollection<SizeDto>GetSizes();
        ICollection<CommentDto>GetGeneralComments();
        double GetPriceForSizeGeneric(string size);
    }
}
