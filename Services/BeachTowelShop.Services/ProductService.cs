﻿using AutoMapper;
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

        public ICollection<CategoryDto> GetAllCategories()
        {
            var data = _appDbContext.Categories.ToList();

            var dataDto = _mapper.Map<List<CategoryDto>>(data);
            return dataDto;

        }

        public ICollection<GalleryProductDto> GetAllProducts()
        {
            //need picture URL,Name
            var products = _appDbContext.Products.ToList();

            var dataDto = _mapper.Map<List<GalleryProductDto>>(products);
            //TODO:refaktor with linq
            foreach (var item in products)
            {
                var pictures = _appDbContext.ProductPictures.Where(p => p.ProductId == item.Id).Select(p => p.Picture.Path).ToList();

                dataDto.Where(p => p.Id == item.Id).FirstOrDefault().PictureList.AddRange(pictures);
            }

            return dataDto;
        }

        public ICollection<GalleryProductDto> GetAllProductsForCategory(string categoryId)
        {
            var productIds = _appDbContext.ProductCategories.Where(c => c.CategoryId == categoryId).Select(p => p.Product).ToList();
            //var products = _appDbContext.Products.Where(p => productIds.Any());
            var dataDto = _mapper.Map<List<GalleryProductDto>>(productIds);
            //TODO:refaktor with linq
            foreach (var item in productIds)
            {
                var pictures = _appDbContext.ProductPictures.Where(p => p.ProductId == item.Id).Select(p => p.Picture.Path).ToList();

                dataDto.Where(p => p.Id == item.Id).FirstOrDefault().PictureList.AddRange(pictures);
            }

            return dataDto;
        }

        public ICollection<SizeDto> GetAllSizes()
        {
            var sizes = _appDbContext.Sizes.ToList();
            var sizesDto = _mapper.Map<List<SizeDto>>(sizes);
            return sizesDto;
        }

        public ICollection<SizeDto> GetAllSizesForProductById(string itemid)
        {
            var sizesForProduct = _appDbContext.ProductSizes.Include(m => m.Size).Where(i => i.ProductId == itemid).ToList();

            var sizeDtoList = _mapper.Map<List<SizeDto>>(sizesForProduct);

            return sizeDtoList;
        }

        public ICollection<CommentDto> GetGeneralComments()
        {
            var generalCommentsList = _appDbContext.Comments.Where(a => a.ProductId == null);
            var generalCommentsDtoList = _mapper.Map<List<CommentDto>>(generalCommentsList);
            return generalCommentsDtoList;
        }

        public double GetPriceForSize(string sizeName)
        {

            var price = _appDbContext.Sizes.Where(a => a.Name == sizeName).Select(b => b.Price).FirstOrDefault();
            return double.Parse(price);
        }

        public ProductDto GetProductById(string itemid)
        {

            var product = _appDbContext.Products.Where(item => item.Id == itemid).FirstOrDefault();
            var productDto = _mapper.Map<ProductDto>(product);
            var pictures = _appDbContext.ProductPictures.Where(p => p.ProductId == productDto.Id).Select(p => p.Picture.Path).ToList();
            productDto.PictureList.AddRange(pictures);
            return productDto;
        }

        public ICollection<ProductDto> GetSimilarProductsById(string itemid)
        {
            var categories = _appDbContext.ProductCategories.Where(c => c.ProductId == itemid).Select(p => p.CategoryId).ToList();
            Random rnd = new Random();
            var similarProducts = _appDbContext.ProductCategories.Where(c => categories.Any(x => c.CategoryId == x)).Select(p => p.Product).Where(x => x.Id != itemid).Take(4).ToList();
            var similarProductsDto = _mapper.Map<List<ProductDto>>(similarProducts);

            //TODO:refaktor with linq
            foreach (var item in similarProductsDto)
            {
                var pictures = _appDbContext.ProductPictures.Where(p => p.ProductId == item.Id).Select(p => p.Picture.Path).ToList();

                similarProductsDto.Where(p => p.Id == item.Id).FirstOrDefault().PictureList.AddRange(pictures);
            }
            return similarProductsDto;
        }

        public ICollection<SizeDto> GetSizes()
        {
            var towelSizes = _appDbContext.Sizes.ToList();
            if (!towelSizes.Any())
            {
                return new List<SizeDto>();
            }
            var towelSizeDto = _mapper.Map<List<SizeDto>>(towelSizes);
            return towelSizeDto;
        }

        public bool VerifyId(string productId)
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