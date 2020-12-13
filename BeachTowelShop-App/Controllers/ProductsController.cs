using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeachTowelShop.Models.Orders;
using BeachTowelShop.Models.Products;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace BeachTowelShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService __productService;
        private readonly IMapper _mapper;
        private readonly IOrderService __orderService;
        private readonly IMemoryCache _cache;
        public ProductsController(IProductService productService, IMapper mapper,IOrderService orderService, IMemoryCache memoryCache)
        {
            __productService = productService;
            _mapper = mapper;
            __orderService = orderService;
            _cache = memoryCache; 
        }
        // GET: Products
        //Works here
       
        [AllowAnonymous]
        [Route("Products")]
        public IActionResult Products()
        {
            
            GalleryProductsViewModel productList;
            if (!_cache.TryGetValue("GalleryProductsViewModel", out productList))
            {
                productList = new GalleryProductsViewModel();
                var allCategories = __productService.GetAllCategories();
                var categoryViewModelList = _mapper.Map<List<CategoryViewModel>>(allCategories).OrderBy(a => a.Name);
              
                productList.AllCategories.AddRange(categoryViewModelList);
                var allProducts = __productService.GetAllProducts();
                var productViewModelList = _mapper.Map<List<GalleryProductViewModel>>(allProducts);
                productList.AllProducts.AddRange(productViewModelList);
                _cache.Set("GalleryProductsViewModel", productList);
            }
            productList = _cache.Get("GalleryProductsViewModel") as GalleryProductsViewModel;

          
           
           
            return View(productList);
        }

        // GET: Products/id?
        [Route("Products/Sort/{id?}")]
      
        public IActionResult GetItemsForCategory(string categoryid)
        {
           
            GalleryProductsViewModel productList;
            GalleryProductsViewModel productList2;
            if (!_cache.TryGetValue("GalleryProductsViewModel", out productList2))
            {
                //TODO:refaktor caching and viewbag
                productList2 = new GalleryProductsViewModel();
                var allCategories = __productService.GetAllCategories();
                var categoryViewModelList = _mapper.Map<List<CategoryViewModel>>(allCategories).OrderBy(a => a.Name);
                var allProducts = __productService.GetAllProductsForCategory(categoryid);
                
                productList2.AllCategories.AddRange(categoryViewModelList);
                _cache.Set("GalleryProductsViewModelFilter",productList2);
               
            }
            if(!_cache.TryGetValue($"GalleryProductsViewModelFilter{categoryid}",out productList))
            {
                 productList = new GalleryProductsViewModel();
                productList2 = _cache.Get("GalleryProductsViewModel") as GalleryProductsViewModel;
               
                productList.AllCategories.AddRange(productList2.AllCategories);
                var allProductsDto = __productService.GetAllProductsForCategory(categoryid);
                var filteredProducts = _mapper.Map<List<GalleryProductViewModel>>(allProductsDto);
                productList.AllProducts.AddRange(filteredProducts);
                _cache.Set($"GalleryProductsViewModelFilter{categoryid}", productList);
            }
          productList=  _cache.Get($"GalleryProductsViewModelFilter{categoryid}") as GalleryProductsViewModel;
          
          
            
            return View("Products", productList);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Item(string itemid)
        {
            
            ProductViewModelList list;
            if (!_cache.TryGetValue($"ProductViewModelList{itemid}",out list))
            {
                list = new ProductViewModelList();
                var IsIdValid = __productService.VerifyId(itemid);
                if (!IsIdValid)
                {
                    return NotFound();
                }
                var productDto = __productService.GetProductById(itemid);
                var productViewModel = _mapper.Map<ProductViewModel>(productDto);
                var similarProductsDto = __productService.GetSimilarProductsById(itemid);
                var similarProducts = _mapper.Map<List<ProductViewModel>>(similarProductsDto);
                var sizesDtoForProduct = __productService.GetAllSizesForProductById(itemid);
                var sizesViewModelForProduct = _mapper.Map<List<SizeViewModel>>(sizesDtoForProduct).OrderBy(x => x.Price);
                list.Product = productViewModel;
                list.SimilarProducts.AddRange(similarProducts);
                list.Product.Sizes.AddRange(sizesViewModelForProduct);
                _cache.Set($"ProductViewModelList{itemid}", list);
            }
            list = _cache.Get($"ProductViewModelList{itemid}") as ProductViewModelList;

            
            return View(list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     
        public IActionResult AddToCart([FromBody] CartViewModel cartViewModels)
        {
            string sessionCookie = "BeachTowelShop-Session";
            if (!Request.Cookies.ContainsKey(sessionCookie))
            {
                Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
            }
            if (Request.Cookies[sessionCookie] == null)
            {
                return View();
            }
            var userId = Request.Cookies[sessionCookie];
            OrderDataViewModel orderDataViewModel = new OrderDataViewModel();
            orderDataViewModel.Count = int.Parse(cartViewModels.Count);
            orderDataViewModel.DesignFolderPath = cartViewModels.DesignFolderPath;
            orderDataViewModel.DesignName = cartViewModels.DesignName;
            if (orderDataViewModel.DesignName == null)
            {
                var values = orderDataViewModel.DesignFolderPath.Split("/");
                orderDataViewModel.DesignFolderPath = values[0];
                orderDataViewModel.DesignName =values[1];
            }
            orderDataViewModel.Size = cartViewModels.Size;
            orderDataViewModel.Price = __productService.GetPriceForSize(orderDataViewModel.Size);
            var doesIdExist = __productService.VerifyId(cartViewModels.ProductId);
            if (!doesIdExist)
            {
                return NotFound();
            }
            orderDataViewModel.ProductId = cartViewModels.ProductId;
            orderDataViewModel.SessionId = Request.Cookies[sessionCookie];
            orderDataViewModel.Sum = orderDataViewModel.Price * orderDataViewModel.Count;
                                                                   
            var userSessionDto = _mapper.Map<UserSessionCartDto>(orderDataViewModel);
           
            __orderService.SaveItemToCart(userSessionDto);

            List<CartViewModel> itemsInCache;
            if (!_cache.TryGetValue($"CartViewModel{userId}", out itemsInCache))
            {
                itemsInCache = new List<CartViewModel>();
                _cache.Set($"CartViewModel{userId}", itemsInCache);
            }
            itemsInCache = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
            var cartItem = _mapper.Map<CartViewModel>(userSessionDto);
            itemsInCache.Add(cartItem);
            _cache.Remove($"CartViewModel{userId}");
            _cache.Set($"CartViewModel{userId}", itemsInCache);

            return Ok("Towel added to the cart");
                 
             
            }
        private void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            

            option.SameSite = SameSiteMode.None;
            option.HttpOnly = true;
            option.Secure = true;
            option.IsEssential = true;

            Response.Cookies.Append(key, value, option);
          
        }
        
    }
}