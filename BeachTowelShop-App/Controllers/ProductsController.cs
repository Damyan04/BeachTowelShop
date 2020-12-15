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
        public IActionResult Products(int startPage=0)
        {
            string sessionCookie = "BeachTowelShop-Session";
            if (!Request.Cookies.ContainsKey(sessionCookie))
            {
                Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
            }
            var pageSize = 20;

            var fromPage = startPage * pageSize;
            
            ViewBag.NextPage = startPage + 1;
            ViewBag.PreviousPage = startPage - 1;

            GalleryProductsViewModel productList;
           
                productList = new GalleryProductsViewModel();
            List<CategoryViewModel> categoryViewModelList;
            if (!_cache.TryGetValue("CategoryViewModel", out categoryViewModelList))
            {
                var allCategories = __productService.GetAllCategories();
                categoryViewModelList = _mapper.Map<List<CategoryViewModel>>(allCategories).OrderBy(a => a.Name).ToList();
                _cache.Set("CategoryViewModel", categoryViewModelList);
               
            }
            categoryViewModelList=_cache.Get("CategoryViewModel") as List<CategoryViewModel>;
            productList.AllCategories.AddRange(categoryViewModelList);
            List<GalleryProductViewModel> productViewModelList;
            if (!_cache.TryGetValue("GalleryProductViewModel", out productViewModelList))
            {
                
                var allProducts = __productService.GetAllProducts();
                 productViewModelList = _mapper.Map<List<GalleryProductViewModel>>(allProducts);
               
                _cache.Set("GalleryProductViewModel", productViewModelList);
            }
            productViewModelList = _cache.Get("GalleryProductViewModel") as List<GalleryProductViewModel>;
            var lastPage = productViewModelList.Count / pageSize;
            if (fromPage >= lastPage)
            {
                ViewBag.NextPage = lastPage;
                ViewBag.PreviousPage = lastPage - 1;
            }

            ViewBag.Action = "Products";
            
            productList.AllProducts = productViewModelList.Skip(fromPage).Take(pageSize).ToList();
            
            
            
           
            return View(productList);
        }

        // GET: Products/id?
        [Route("Products/Sort")]
      
        public IActionResult GetItemsForCategory(string categoryid, int startPage = 0)
        {
            var pageSize = 20;

            var fromPage = startPage * pageSize;

            ViewBag.NextPage = startPage + 1;
            ViewBag.PreviousPage = startPage - 1;
            GalleryProductsViewModel productList;
           

            productList = new GalleryProductsViewModel();
            List<CategoryViewModel> categoryViewModelList;
            if (!_cache.TryGetValue("CategoryViewModel", out categoryViewModelList))
            {
                var allCategories = __productService.GetAllCategories();
                categoryViewModelList = _mapper.Map<List<CategoryViewModel>>(allCategories).OrderBy(a => a.Name).ToList();
                _cache.Set("CategoryViewModel", categoryViewModelList);

            }
            categoryViewModelList = _cache.Get("CategoryViewModel") as List<CategoryViewModel>;
            productList.AllCategories.AddRange(categoryViewModelList);
            List<GalleryProductViewModel> productViewModelList;
            if (!_cache.TryGetValue($"GalleryProductViewModelFiler{categoryid}", out productViewModelList))
            {

                var allProducts =__productService.GetAllProductsForCategory(categoryid);
                productViewModelList = _mapper.Map<List<GalleryProductViewModel>>(allProducts);

                _cache.Set($"GalleryProductViewModelFilter{categoryid}", productViewModelList);
            }
            productViewModelList = _cache.Get($"GalleryProductViewModelFilter{categoryid}") as List<GalleryProductViewModel>;
            var lastPage = productViewModelList.Count / pageSize;
            if (fromPage >= lastPage)
            {
                ViewBag.NextPage = lastPage;
                ViewBag.PreviousPage = lastPage - 1;
            }

            ViewBag.CategoryId = $"{categoryid}";
            productList.AllProducts = productViewModelList.Skip(fromPage).Take(pageSize).ToList();
            ViewBag.Action = "Sort";
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
        [Route("Products/AddToCart")]
        public IActionResult AddToCart([FromBody] CartViewModel cartViewModels)
        {
            string sessionCookie = "BeachTowelShop-Session";
            if (!Request.Cookies.ContainsKey(sessionCookie))
            {
                Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
            }
            if (Request.Cookies[sessionCookie] == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            double count = 0;
            var canConvert = double.TryParse(cartViewModels.Count, out count);
            if (!canConvert)
            {
                cartViewModels.Count = "1";
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
            var doesIdExist = __productService.VerifyId(cartViewModels.ProductId);
            if (!doesIdExist)
            {
                return NotFound();
            }
            orderDataViewModel.Size = cartViewModels.Size;
           
            
            
            orderDataViewModel.Price = __productService.GetPriceForSize(orderDataViewModel.Size, cartViewModels.ProductId);
            orderDataViewModel.ProductId = cartViewModels.ProductId;
            orderDataViewModel.SessionId = Request.Cookies[sessionCookie];
            orderDataViewModel.Sum = orderDataViewModel.Price * orderDataViewModel.Count;
                                                                   
            var userSessionDto = _mapper.Map<UserSessionCartDto>(orderDataViewModel);
           
          var updateItem=  __orderService.SaveItemToCart(userSessionDto);

            List<CartViewModel> itemsInCache;
            if (!_cache.TryGetValue($"CartViewModel{userId}", out itemsInCache))
            {
                itemsInCache = new List<CartViewModel>();
               _cache.Set($"CartViewModel{userId}", itemsInCache);
            }
            itemsInCache = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
            var cartItem = _mapper.Map<CartViewModel>(updateItem);
           var itemToRemove= itemsInCache.Where(a => a.SessionId == updateItem.UserSessionId && a.ProductId == updateItem.ProductId&&a.Size==updateItem.Size).FirstOrDefault();
            if (itemToRemove != null)
            {
                itemsInCache.Remove(itemToRemove);
            }
            itemsInCache.Add(cartItem);
            //_cache.Remove($"CartViewModel{userId}");
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

            option.SameSite = SameSiteMode.Strict;


            Response.Cookies.Append(key, value, option);
          
        }
        
    }
}