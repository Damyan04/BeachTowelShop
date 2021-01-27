using AutoMapper;
using AutoMapper.QueryableExtensions;
using BeachTowelShop.Data;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTowelShop.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IMapper _mapper;
        public ProductService(ApplicationDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<CategoryDto>> GetAllCategories()
        {
            var data = await _appDbContext.Categories.ToListAsync();

            var dataDto = _mapper.Map<List<CategoryDto>>(data);
            return dataDto;

        }

        public async Task<List<string>> GetAllPicturePaths()
        {
            var products = await _appDbContext.ProductPictures.Select(a => a.Picture.Path).Distinct().ToListAsync();
            return products;
        }

        public async Task<ICollection<GalleryProductDto>> GetAllProducts()
        {
            //need picture URL,Name
            var products = await _appDbContext.Products.ToListAsync();
            
            
            
        var dataDto = _mapper.Map<List<GalleryProductDto>>(products);
            foreach (var item in dataDto)
            {
                var lowPriceList = await _appDbContext.ProductSizes.Where(a => a.ProductId == item.Id&&a.Price>0).Select(a => a.Price).ToListAsync();
                double lowPrice = 0;
                double highPrice = 0;
                if (lowPriceList.Count > 0)
                {
                    lowPrice = lowPriceList.Min(p => p);
                }
                var highPriceList = await _appDbContext.ProductSizes.Where(a => a.ProductId == item.Id&&a.Price>0).Select(a => a.Price).ToListAsync();
                if (highPriceList.Count > 0)
                {
                    highPrice = lowPriceList.Max(p => p);
                }
                item.LowestPrice = lowPrice;
                item.HighPrice = highPrice;
            }
            var pictures = await _appDbContext.ProductPictures.Include(a => a.Picture).ToListAsync();
            //TODO:refaktor with linq
            foreach (var item in products)
            {
                var picturesPath = pictures.Where(p => p.ProductId == item.Id).Select(p => p.Picture.Path).ToList();
                
                dataDto.Where(p => p.Id == item.Id).FirstOrDefault().PictureList.AddRange(picturesPath);
            }

            return dataDto;
        }

        public async Task<ICollection<GalleryProductDto>> GetAllProductsForCategory(string categoryId)
        {
            var productIds = await _appDbContext.ProductCategories.Where(c => c.CategoryId == categoryId).Select(p => p.Product).ToListAsync();
           
            //var products = _appDbContext.Products.Where(p => productIds.Any());
            var dataDto = _mapper.Map<List<GalleryProductDto>>(productIds);
            //TODO:refaktor with linq
            foreach (var item in dataDto)
            {
                var lowPriceList = await _appDbContext.ProductSizes.Where(a => a.ProductId == item.Id && a.Price > 0).Select(a => a.Price).ToListAsync();
                double lowPrice = 0;
                double highPrice = 0;
                if (lowPriceList.Count > 0)
                {
                    lowPrice = lowPriceList.Min(p => p);
                }
                var highPriceList = await _appDbContext.ProductSizes.Where(a => a.ProductId == item.Id && a.Price > 0).Select(a => a.Price).ToListAsync();
                if (highPriceList.Count > 0)
                {
                    highPrice = lowPriceList.Max(p => p);
                }
                item.LowestPrice = lowPrice;
                item.HighPrice = highPrice;
            }
            var pictures = await _appDbContext.ProductPictures.Include(a => a.Picture).ToListAsync();
            foreach (var item in productIds)
            {
                var picturesPath = pictures.Where(p => p.ProductId == item.Id).Select(p => p.Picture.Path).ToList();

                dataDto.Where(p => p.Id == item.Id).FirstOrDefault().PictureList.AddRange(picturesPath);
            }

            return dataDto;
        }

        public async Task<ICollection<SizeDto>> GetAllSizes()
        {
            var sizes = await _appDbContext.Sizes.ToListAsync();
            var sizesDto = _mapper.Map<List<SizeDto>>(sizes);
            return sizesDto;
        }

        public async Task<ICollection<SizeDto>> GetAllSizesForProductById(string itemid)
        {
            var sizesForProduct = await _appDbContext.ProductSizes.Include(m => m.Size).Where(i => i.ProductId == itemid).ToListAsync();

            var sizeDtoList = _mapper.Map<List<SizeDto>>(sizesForProduct);

            return sizeDtoList;
        }

        public async Task<ICollection<CommentDto>> GetGeneralComments()
        {
            var generalCommentsList = await _appDbContext.Comments.Where(a => a.ProductId == null).ToListAsync();
            var generalCommentsDtoList = _mapper.Map<List<CommentDto>>(generalCommentsList);
            return generalCommentsDtoList;
        }

        public async Task<double> GetPriceForSize(string sizeName,string productId)
        {

            var sizeId = _appDbContext.Sizes.Where(a => a.Name == sizeName).Select(b => b.Id).FirstOrDefault();
            var towelPrice = _appDbContext.ProductSizes.Where(a => a.SizeId == sizeId && a.ProductId == productId).Select(a => a.Price).FirstOrDefault();
            return towelPrice;
        }

        public async Task<double> GetPriceForSizeGeneric(string size)
        {
           return _appDbContext.Sizes.Where(a => a.Name == size).Select(b => b.Price).FirstOrDefault();
           
        }

        public async Task<ProductDto> GetProductById(string itemid)
        {

            var product = _appDbContext.Products.Where(item => item.Id == itemid).FirstOrDefault();
            var productDto = _mapper.Map<ProductDto>(product);
            var pictures = await _appDbContext.ProductPictures.Where(p => p.ProductId == productDto.Id).Select(p => p.Picture.Path).ToListAsync();
            productDto.PictureList.AddRange(pictures);
            return productDto;
        }

        public async Task<ICollection<ProductDto>> GetSimilarProductsById(string itemid)
        {
            var categories = await _appDbContext.ProductCategories.Where(c => c.ProductId == itemid).Select(p => p.CategoryId).ToListAsync();
            Random rnd = new Random();
            var similarProducts = await _appDbContext.ProductCategories.Where(c => categories.Any(x => c.CategoryId == x)).Select(p => p.Product).Where(x => x.Id != itemid).Take(4).ToListAsync();
            var similarProductsDto = _mapper.Map<List<ProductDto>>(similarProducts);

            //TODO:refaktor with linq
            foreach (var item in similarProductsDto)
            {
                var pictures = await _appDbContext.ProductPictures.Where(p => p.ProductId == item.Id).Select(p => p.Picture.Path).ToListAsync();

                similarProductsDto.Where(p => p.Id == item.Id).FirstOrDefault().PictureList.AddRange(pictures);
            }
            return similarProductsDto;
        }

        public async Task<ICollection<SizeDto>> GetSizes()
        {
            var towelSizes = await _appDbContext.Sizes.ToListAsync();
            if (!towelSizes.Any())
            {
                return new List<SizeDto>();
            }
            var towelSizeDto = _mapper.Map<List<SizeDto>>(towelSizes);
            return towelSizeDto;
        }

        public async Task<bool> VerifyId(string productId)
        {
            var item = _appDbContext.Products.Where(a => a.Id == productId).FirstOrDefault();
            if (item != null)
            {
                return true;
            }
            return false;
        }
    }
}
