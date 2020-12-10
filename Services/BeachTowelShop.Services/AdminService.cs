using AutoMapper;
using BeachTowelShop.Data;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeachTowelShop.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IMapper _mapper;
        public AdminService(ApplicationDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public AdminProductDto GetAdminProduct(string id)
        {

            var product = _appDbContext.Products.Where(item => item.Id == id).FirstOrDefault();
            var productDto = _mapper.Map<AdminProductDto>(product);
            var pictures = _appDbContext.ProductPictures.Where(p => p.ProductId == productDto.Id).Select(p => p.Picture.Path).ToList();
            productDto.PictureList.AddRange(pictures);
            var sizesAndPrices = GetAllSizesWithPrice(id);
            productDto.SizesPricesList.AddRange(sizesAndPrices);
            var categories = _appDbContext.ProductCategories.Where(a => a.ProductId == id).Select(a=>a.Category).ToList();
            var categoryDto = _mapper.Map <List<CategoryDto>>(categories);
            productDto.CategoryViews.AddRange(categoryDto);
            return productDto;
        }

        public IEnumerable<PartialOrderDto> GetAllOrders()
        {
            var odersDtoList = new List<PartialOrderDto>();
            var ordersList = _appDbContext.Orders.Select(a => new { a.Id, a.Sum, a.Status }).ToList();
          ordersList.ForEach(a => odersDtoList.Add(new PartialOrderDto { Id = a.Id, Status = a.Status, Sum = a.Sum }));
            ordersList.OrderByDescending(a => a.Status);
            return odersDtoList;
        }

        public List<AdminProductDto> GetAllProductsForAdmin()
        {
            var allProducts = _appDbContext.Products.ToList();
            var allProductsDto = _mapper.Map<List<AdminProductDto>>(allProducts);
            var pictures = _appDbContext.ProductPictures.Include(a => a.Picture).ToList();
            foreach (var item in allProducts)
            {
                var picturesPath = pictures.Where(p => p.ProductId == item.Id).Select(p => p.Picture.Path).ToList();

                allProductsDto.Where(p => p.Id == item.Id).FirstOrDefault().PictureList.AddRange(picturesPath);
            }
            return allProductsDto;
        }

        public List<SizeWithPriceDto> GetAllSizesWithPrice(string id)
        {
            var allSizesForProduct = _appDbContext.ProductSizes.Include(a=>a.Size).Where(a => a.ProductId == id).ToList();
            var allSizesDto = _mapper.Map<List<SizeWithPriceDto>>(allSizesForProduct);
            return allSizesDto;
            
        }

        public FullOrderDto GetOrderById(string id)
        {
            var item = _appDbContext.Orders.Where(a => a.Id == id).FirstOrDefault();
            var designItems = _appDbContext.CartItems.Where(a => a.OrderId == id).ToList();
            var designItemsDto = _mapper.Map<List<UserSessionCartDto>>(designItems);
            var textProperties = _appDbContext.TextProperties.Where(a => a.OrderId == id).ToList();
            var textForDesignsDto = _mapper.Map<List<UserTextSessionDto>>(textProperties);
            var itemDto = _mapper.Map<FullOrderDto>(item);
            itemDto.FinishedDesings.AddRange(designItemsDto);
            itemDto.TextForDesings.AddRange(textForDesignsDto);
            //TODO: map the LIST of DESIGNS AND TEXTS
            return itemDto;
        }

        public void SaveAdminProductSize(SizeWithPriceDto productSize,string productId)
        {
            var item = _appDbContext.ProductSizes.Where(a => a.SizeId == productSize.Id && a.ProductId == productId).FirstOrDefault();
            
            if (item != null)
            {
                var newPrice = productSize.Price;
                item.Price = newPrice;
            }
            _appDbContext.SaveChanges();
        }

        public void SaveSizesToDb(SizeDto sizeDto)
        {
            // var sizeItem=_mapper.Map<Size>(sizeDto);

            
            var item = _appDbContext.Sizes.Where(a => a.Id == sizeDto.Id).FirstOrDefault();
            if (item != null)
            {
                //TODO:Make it not string in the db
                item.Name = sizeDto.Name;
                item.Price = sizeDto.Price.ToString();
            }

            _appDbContext.SaveChanges();

        }
    }
}
