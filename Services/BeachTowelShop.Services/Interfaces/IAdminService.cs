using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Interfaces
{
   public interface IAdminService
    {
        void SaveSizesToDb(SizeDto sizeDto);

        IEnumerable<PartialOrderDto> GetAllOrders();

        FullOrderDto GetOrderById(string id);

        List<SizeWithPriceDto> GetAllSizesWithPrice(string id);

        List<AdminProductDto> GetAllProductsForAdmin();

        AdminProductDto GetAdminProduct(string id);
        void SaveAdminProductSize(SizeWithPriceDto productSize,string productId);

       void CreateProduct(AdminProductDto productDto);
        void UpdateItem(AdminProductDto productDto);
        void DeleteItemById(string id);
        void CreateCategoryAndPictureForItem(List<PictureDto> picturePathList, List<string> categoryList,string id);
        public void DeleteCategory(string productId,string categoryId);
        public void DeletePicture(string productId, string picturePathId);
        void ChangeStatus(string id, string status);
    }
}
