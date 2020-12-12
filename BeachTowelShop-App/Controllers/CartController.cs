using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeachTowelShop.Models.Cart;
using BeachTowelShop.Models.Orders;
using BeachTowelShop.Models.Products;
using BeachTowelShop.Services;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace BeachTowelShop.Controllers
{
    public class CartController : Controller
    {
        
        private readonly IMapper _mapper;
        private readonly IOrderService __orderService;
        private readonly IProductService __productService;
        private readonly IMemoryCache _cache;
        public CartController(IMapper mapper, IOrderService orderService, IProductService productService, IMemoryCache memoryCache)
        {
           
            _mapper = mapper;
            __orderService = orderService;
            __productService = productService;
            _cache = memoryCache;
        }

        // GET: Cart
        public ActionResult Index()
        {
            string cookie = "BeachTowelShop-Session";
            if (!Request.Cookies.ContainsKey(cookie))
            {
                Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
            }
            if (Request.Cookies[cookie] == null)
            {
                return View();
            }
            var userId = Request.Cookies[cookie];
            bool hasItems;
          


            hasItems = __orderService.HasItems(userId);
                
            
          
                if (!hasItems)
            {
                var viewList = new List<CartViewModel>();
                return View(viewList);
            }
            
            List<CartViewModel> viewListViewModel;
            if(!_cache.TryGetValue($"CartViewModel{userId}", out viewListViewModel))
            {
            var viewListDto = __orderService.GetItemsInCart(userId);
             viewListViewModel = _mapper.Map<List<CartViewModel>>(viewListDto);
                _cache.Set($"CartViewModel{userId}", viewListViewModel);
            }
            viewListViewModel = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
            return View(viewListViewModel);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteFromCart([FromBody]CartItemViewModel cartItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request 400");
            }
            string cookie = "BeachTowelShop-Session";
            if (!Request.Cookies.ContainsKey(cookie))
            {
                Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
            }
            if (Request.Cookies[cookie] == null)
            {
                return View();
            }

            var userId = Request.Cookies[cookie];
            var productId = cartItem.ProductId;
            var cartItemDto = _mapper.Map<UserSessionCartDto>(cartItem);
            cartItem.ImgName = __orderService.GetItemFolderPath(productId);
            __orderService.DeleteItemFromCart(userId, cartItemDto);

         //   var cartViewModel = _mapper.Map<CartViewModel>(cartItemDto);
            var itemsInCache = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
           var item= itemsInCache.Where(a=>a.ProductId== cartItemDto.ProductId&&a.SessionId== userId).FirstOrDefault();
            itemsInCache.Remove(item);
            if (itemsInCache.Count != 0)
            {
                _cache.Remove($"CartViewModel{userId}");
                _cache.Set($"CartViewModel{userId}", itemsInCache);
            }
            else
            {
                _cache.Remove($"CartViewModel{userId}");
            }
            
            DeleteFromServerAsync(productId, cartItem);
            //TODO:Make it async as it is irelevent for the user
            
           
            return Ok("Removed from Cart");
        }

        

        [HttpPut]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult ChangeInCart([FromBody]CartItemViewModel cartItem)
        {
           
          
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request 400");
            }
            string cookie = "BeachTowelShop-Session";
            if (!Request.Cookies.ContainsKey(cookie))
            {
                Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
            }
            if (Request.Cookies[cookie] == null)
            {
                return View();
            }
            double price = __productService.GetPriceForSize(cartItem.Size);
            cartItem.Sum = int.Parse(cartItem.Count) * price;
            var userId = Request.Cookies[cookie];
            //TODO pass sum
            var userCartDto=_mapper.Map<UserSessionCartDto>(cartItem);
            __orderService.UpdateCart(userId, userCartDto);
            var itemsInCache = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
            var item = itemsInCache.Where(a => a.ProductId == userCartDto.ProductId && a.SessionId == userId).FirstOrDefault();
            var updatedItem = _mapper.Map<CartViewModel>(userCartDto);
            updatedItem.SessionId = userId;
            updatedItem.DesignFolderPath = item.DesignFolderPath;
            updatedItem.DesignName = item.DesignName;
            itemsInCache.Remove(item);
            itemsInCache.Add(updatedItem);
            
            _cache.Remove($"CartViewModel{userId}");
            
            _cache.Set($"CartViewModel{userId}", itemsInCache);
            return Ok("Ok");

           
           
            
        }


        private async Task DeleteFromServerAsync (string productId, CartItemViewModel cartItem)
        {
            
            if (productId.Contains(".png") && cartItem.ImgName.Contains("received"))
            {

                try
                {

                    string rootFolderPath = $"C:/Users/damot/source/repos/BeachTowelShop/BeachTowelShop-App/wwwroot/{cartItem.ImgName}/";
                    string filesToDelete = $"{productId}";
                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                    foreach (string file in fileList)
                    {
                        System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                        System.IO.File.Delete(file);
                    }
                }
                catch (Exception e)
                {

                    //console.log (e.Message.ToString());
                }
            }
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