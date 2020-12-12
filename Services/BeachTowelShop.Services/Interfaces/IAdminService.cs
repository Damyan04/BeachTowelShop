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
    }
}
