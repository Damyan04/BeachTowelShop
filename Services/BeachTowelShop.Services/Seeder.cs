using BeachTowelShop.Data;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Data.Models.Interfaces;
using BeachTowelShop.Services.Interfaces;
using JsonNet.ContractResolvers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeachTowelShop.Services
{
    public class Seeder : ISeeder
    {
        //Use Automapper,DTO's 
        //Need to change the Type to seed other Tables
        public static void Seedit(string jsonDataProduct, string jsonDataPicture, string jsonDataSize, string jsonDataCategory,
                          IServiceProvider serviceProvider)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
            List<Product> products =
             JsonConvert.DeserializeObject<List<Product>>(
               jsonDataProduct, settings);
            List<Picture> pictures =
             JsonConvert.DeserializeObject<List<Picture>>(
               jsonDataPicture, settings);
            List<Size> sizes =
             JsonConvert.DeserializeObject<List<Size>>(
               jsonDataSize, settings);
            List<Category> categories =
             JsonConvert.DeserializeObject<List<Category>>(
               jsonDataCategory, settings);
            
            using (
             var serviceScope = serviceProvider
               .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope
                              .ServiceProvider.GetService<ApplicationDbContext>();
                if (!context.Categories.Any() && !context.Products.Any() && !context.Pictures.Any() && !context.Sizes.Any())
                {
                    context.Products.AddRange(products);
                    context.Pictures.AddRange(pictures);
                    context.Sizes.AddRange(sizes);
                    context.Categories.AddRange(categories);

                    foreach (var product in products)
                    {
                       
                        foreach (var size in sizes)
                        {
                            var productSize = new ProductSize();
                            productSize.ProductId = product.Id;
                            productSize.SizeId = size.Id;
                            switch (size.Name)
                            {
                                case"50x30": productSize.Price = 10.00;break;
                                case "70x50": productSize.Price = 20.00; break;
                                case "160x80": productSize.Price = 44.00; break;
                                case "140x70": productSize.Price = 29.00; break;
                                case "180x100": productSize.Price = 49.00; break;
                                default:
                                    break;
                            }
                            context.ProductSizes.Add(productSize);
                        }
                        
                        var productCategory = new ProductCategory();
                        productCategory.ProductId = product.Id;
                        productCategory.CategoryId = categories[1].Id;
                        context.ProductCategories.Add(productCategory);
                        var productPictures = new ProductPicture();
                        productPictures.ProductId = product.Id;
                        productPictures.PictureId = pictures[0].Id;
                        context.ProductPictures.Add(productPictures);
                        
                    }
                    

                    context.SaveChanges();
                }

            }
        }
    }
}
