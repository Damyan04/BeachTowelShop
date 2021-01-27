using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeachTowelShop.Services.Interfaces
{
   public interface IProductService
    {
       Task<ICollection<CategoryDto>> GetAllCategories();
       Task <ICollection<GalleryProductDto>> GetAllProducts();

       Task<ICollection<GalleryProductDto>> GetAllProductsForCategory(string categoryId);

        Task<ProductDto> GetProductById(string itemid);

        Task<ICollection<ProductDto>> GetSimilarProductsById(string itemid);
        Task<ICollection<SizeDto>> GetAllSizesForProductById(string itemid);

        Task<ICollection<SizeDto>> GetAllSizes();

        Task<double> GetPriceForSize(string size, string productId);
        Task<bool> VerifyId(string productId);
        Task<ICollection<SizeDto>>GetSizes();
        Task<ICollection<CommentDto>>GetGeneralComments();
        Task<double> GetPriceForSizeGeneric(string size);
        Task<List<string>> GetAllPicturePaths();
    }
}
