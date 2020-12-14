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

        public void CreateCategoryAndPictureForItem(List<PictureDto> picturePathList, List<string> categoryList,string id)
        {
            var product = _appDbContext.Products.Where(a => a.Id == id).FirstOrDefault();
            foreach (var item in categoryList)
            {
                
                var doesCategoryExistsForProduct = _appDbContext.ProductCategories.Where(a => a.ProductId == id && a.Category.Name == item).Any();
                if (doesCategoryExistsForProduct)
                {
                    return;
                }
                var doesCategoryExist = _appDbContext.Categories.Where(a => a.Name == item).FirstOrDefault();
                if (doesCategoryExist == null)
                {
                    var category = new Category() { Name = item };
                    _appDbContext.Categories.Add(category);
                    _appDbContext.SaveChanges();
                   
                   

                }
                var cId = _appDbContext.Categories.Where(a => a.Name == item).Select(a => a.Id).FirstOrDefault();
                var pC = new ProductCategory() { CategoryId = cId, ProductId = id };
              
                  product.ProductCategories.Add(pC);
               
              

            }
            var picture = _mapper.Map<List<Picture>>(picturePathList);
            foreach (var pic in picture)
            {
                var doesPicExist = _appDbContext.Pictures.Where(a => a.Path == pic.Path).Any();
                if (!doesPicExist)
                {
                    _appDbContext.Pictures.Add(pic);
                    _appDbContext.SaveChanges();
                }
                
                var productPicture = new ProductPicture() { PictureId = pic.Id, ProductId = id };
                product.ProductPictures.Add(productPicture);

            }
            _appDbContext.SaveChanges();

        }
       
        public void CreateProduct(AdminProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.Id = Guid.NewGuid().ToString();
            var picture = _mapper.Map<List<Picture>>(productDto.PictureList);
            
            var categories = _mapper.Map<List<Category>>(productDto.CategoryViews);
            var id = _appDbContext.Products.Select(a=>a.Id).FirstOrDefault();
            var siezesPrices = GetAllSizesWithPrice(id);
            var sizes = _mapper.Map<List<Size>>(productDto.SizesPricesList);
            foreach (var pic in picture)
            {
                var doesPicExist = _appDbContext.Pictures.Where(a => a.Path == pic.Path).Any();
                if (!doesPicExist)
                {
                    _appDbContext.Pictures.Add(pic);
                    _appDbContext.SaveChanges();
                }
               
                var productPicture = new ProductPicture() { PictureId = pic.Id, ProductId = product.Id };
                product.ProductPictures.Add(productPicture);
                
            }
            foreach (var size in siezesPrices)
            {
                var sizesAndPrices = new ProductSize() { ProductId = product.Id, SizeId = size.Id, Price = size.Price };
                product.ProductSizes.Add(sizesAndPrices);
            }
            foreach (var c in categories)
            {
                if (!_appDbContext.Categories.Where(a => a.Name == c.Name).Any())
                {
                    var category = new Category() { Name = c.Name };
                    _appDbContext.Categories.Add(category);
                    _appDbContext.SaveChanges();
                }

                var doesCategoryExistsForProduct = _appDbContext.ProductCategories.Where(a => a.ProductId == id && a.Category.Name == c.Name).Any();
                if (!doesCategoryExistsForProduct)
                {
                    var cId = _appDbContext.Categories.Where(a => a.Name == c.Name).Select(a => a.Id).FirstOrDefault();
                    var pC = new ProductCategory() { CategoryId = cId, ProductId = product.Id };
                    product.ProductCategories.Add(pC);
                }
                
                
            }
            if (siezesPrices.Count == 0)
            {
                var allGenericSizes= _appDbContext.Sizes.ToList();
                foreach (var size in allGenericSizes)
                {
                    var genericSizesWithPrice = new ProductSize { ProductId = product.Id, SizeId = size.Id, Price = double.Parse(size.Price) };
                    product.ProductSizes.Add(genericSizesWithPrice);
                }
               
            }
          
          
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
        }

        public void DeleteCategory(string productId, string categoryId)
        {
            var categoryPair = _appDbContext.ProductCategories.Where(a => a.ProductId == productId && a.CategoryId == categoryId).FirstOrDefault();
            var allCategories = _appDbContext.ProductCategories.Where(a => a.ProductId == productId).ToList();
            // var product=_appDbContext.Products.
            if (categoryPair != null &&allCategories.Count>1)
            {
                _appDbContext.ProductCategories.Remove(categoryPair);
                _appDbContext.SaveChanges();
                var doesCategoryExists = _appDbContext.ProductPictures.Where(a => a.PictureId == categoryId).FirstOrDefault();
                if (doesCategoryExists == null)
                {
                    var category = _appDbContext.Categories.Where(a => a.Id == categoryId).FirstOrDefault();
                    _appDbContext.Categories.Remove(category);
                    _appDbContext.SaveChanges();
                }
            }
        }

        public void DeleteItemById(string id)
        {
            //TODO if picture is not assosiated with a item delete the picture/category/
            var itemToDelete = _appDbContext.Products.Where(a => a.Id == id).FirstOrDefault();
            if (itemToDelete != null)
            {
                var allCategoriesForTheProduct = _appDbContext.ProductCategories.Where(a => a.ProductId == id).Select(a => a.Category).ToList();
                var allPicturesForProduct = _appDbContext.ProductPictures.Where(a => a.ProductId == id).Select(a => a.Picture).ToList();
                _appDbContext.Products.Remove(itemToDelete);
                _appDbContext.SaveChanges();
                foreach (var item in allCategoriesForTheProduct)
                {
                    var isCategoryEmpty = _appDbContext.ProductCategories.Where(a => a.CategoryId == item.Id).ToList();
                    if (isCategoryEmpty.Count == 0)
                    {
                        _appDbContext.Categories.Remove(item);
                        _appDbContext.SaveChanges();
                    }
                }
                foreach (var item in allPicturesForProduct)
                {
                    var isCategoryEmpty = _appDbContext.ProductPictures.Where(a => a.PictureId == item.Id).ToList();
                    if (isCategoryEmpty.Count == 0)
                    {
                        _appDbContext.Pictures.Remove(item);
                        _appDbContext.SaveChanges();
                    }
                }


            }
        }

        public void DeletePicture(string productId, string picturePathId)
        {
            var picturePair = _appDbContext.ProductPictures.Where(a => a.ProductId == productId && a.PictureId == picturePathId).FirstOrDefault();
            var allPicturePairs = _appDbContext.ProductPictures.Where(a => a.ProductId == productId).ToList();
            // var product=_appDbContext.Products.
            if (picturePair != null&&allPicturePairs.Count>1)
            {
                _appDbContext.ProductPictures.Remove(picturePair);
                _appDbContext.SaveChanges();
                var doesPictureExists=_appDbContext.ProductPictures.Where(a=>a.PictureId == picturePathId).FirstOrDefault();
                if (doesPictureExists == null)
                {
                 var picture=   _appDbContext.Pictures.Where(a => a.Id == picturePathId).FirstOrDefault();
                    _appDbContext.Pictures.Remove(picture);
                    _appDbContext.SaveChanges();
                }
            }
        }

        public AdminProductDto GetAdminProduct(string id)
        {

            var product = _appDbContext.Products.Where(item => item.Id == id).FirstOrDefault();
            var productDto = _mapper.Map<AdminProductDto>(product);
            var pictures = _appDbContext.ProductPictures.Where(p => p.ProductId == productDto.Id).Select(p => p.Picture).ToList();
            var pictureDto = _mapper.Map<List<PictureDto>>(pictures);
            productDto.PictureList.AddRange(pictureDto);
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
                var picturesPath = pictures.Where(p => p.ProductId == item.Id).Select(p => p.Picture).ToList();
                var picturesDto = _mapper.Map<List<PictureDto>>(picturesPath);
                allProductsDto.Where(p => p.Id == item.Id).FirstOrDefault().PictureList.AddRange(picturesDto);
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

        public void UpdateItem(AdminProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var pictures = _mapper.Map<List<Picture>>(productDto.PictureList);
            var categories = _mapper.Map<List<Category>>(productDto.CategoryViews);
            var prices = new List<ProductSize>();
            foreach (var item in productDto.SizesPricesList)
            {
                var price = new ProductSize() { Price = item.Price, ProductId = productDto.Id, SizeId = item.Id};
                prices.Add(price);
            }
            foreach (var picture in pictures)
            {
                _appDbContext.Pictures.Update(picture);
                _appDbContext.SaveChanges();
            }
            foreach (var category in categories)
            {
                _appDbContext.Categories.Update(category);
                _appDbContext.SaveChanges();
            }
            foreach (var price in prices)
            {
                
                _appDbContext.ProductSizes.Update(price);
                _appDbContext.SaveChanges();
            }
            _appDbContext.Products.Update(product);
            _appDbContext.SaveChanges();


        }
    }
}
