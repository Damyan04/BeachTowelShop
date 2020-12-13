using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using BeachTowelShop.Models.Orders;
using BeachTowelShop.Models.Products;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BeachTowelShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductService __productService;
        private readonly IMapper _mapper;
        private IHostEnvironment _hostingEnvironment;
        private readonly IOrderService __orderService;
        private readonly IMemoryCache _cache;
        public OrderController(IProductService productService, IMapper mapper, IHostEnvironment environment, IOrderService orderService, IMemoryCache memoryCache)
        {
            __productService = productService;
            _mapper = mapper;
            _hostingEnvironment = environment;
            __orderService = orderService;
            _cache = memoryCache;
        }
        // GET: Order


        public IActionResult Page(int id, UserDetailsViewModel detailsViewModel)
        {
            if (id < 1 || id > 3)
            {
                id = 1;
            }
            if (detailsViewModel == null)
            {
                detailsViewModel = new UserDetailsViewModel();
            }
            ViewData["id"] = id;
            ViewData["error"] = "";
            
            switch (id)
            {
                case 1:
                    OrderProductViewModel orderProductViewmodel;
                    if (!_cache.TryGetValue("OrderProductViewModel", out orderProductViewmodel))
                    {

                        var sizesDto = __productService.GetAllSizes();
                        var sizesViewModelForProduct = _mapper.Map<List<SizeViewModel>>(sizesDto).OrderBy(x => x.SizeName);
                        orderProductViewmodel = new OrderProductViewModel();
                        orderProductViewmodel.SizeList.AddRange(sizesViewModelForProduct);

                        _cache.Set("OrderProductViewModel", orderProductViewmodel);
                    }
                    orderProductViewmodel = _cache.Get("OrderProductViewModel") as OrderProductViewModel;
                    return View(orderProductViewmodel);
                  
                case 2:
                    string cookie = "BeachTowelShop-Session";

                    if (!Request.Cookies.ContainsKey(cookie))
                    {
                        Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
                    }

                    var userId = Request.Cookies[cookie];
                    var cartHasItems = false;
                    cartHasItems = __orderService.HasItems(userId);

                    if (Request.Cookies.ContainsKey("BeachTowelShop-Session") && cartHasItems)
                    {
                        List<CartViewModel> viewListViewModel;
                        //string cookieValueFromReq = null;
                        if (!_cache.TryGetValue($"CartViewModel{userId}", out viewListViewModel))
                        {
                            var viewListDto = __orderService.GetItemsInCart(userId);
                            viewListViewModel = _mapper.Map<List<CartViewModel>>(viewListDto);


                            detailsViewModel.CartViewModelList.AddRange(viewListViewModel);
                            _cache.Set($"CartViewModel{userId}", viewListViewModel);
                        }

                        detailsViewModel.CartViewModelList = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
                        return View(detailsViewModel);


                    }
                    ////TODO: Need something better
                    else
                    {
                        ViewData["id"] = 1;

                        OrderProductViewModel orderProductViewmodel1;
                        if (!_cache.TryGetValue("OrderProductViewModel", out orderProductViewmodel1))
                        {

                            var sizesDto = __productService.GetAllSizes();
                            var sizesViewModelForProduct = _mapper.Map<List<SizeViewModel>>(sizesDto).OrderBy(x => x.SizeName);
                            orderProductViewmodel1 = new OrderProductViewModel();
                            orderProductViewmodel1.SizeList.AddRange(sizesViewModelForProduct);

                            _cache.Set("OrderProductViewModel", orderProductViewmodel1);
                        }
                        orderProductViewmodel1 = _cache.Get("OrderProductViewModel") as OrderProductViewModel;
                        return View(orderProductViewmodel1);
                    }

                    //TODO: check if cart has items
                   
                case 3:
                    cookie = "BeachTowelShop-Session";

                    if (!Request.Cookies.ContainsKey(cookie))
                    {
                        Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
                    }

                     userId = Request.Cookies[cookie];
                    if (userId != null)
                    {
                        if (!ModelState.IsValid)
                        {
                            List<CartViewModel> viewListViewModel;
                            if (!_cache.TryGetValue($"CartViewModel{userId}", out viewListViewModel))
                            {
                                var viewListDto = __orderService.GetItemsInCart(userId);
                                viewListViewModel = _mapper.Map<List<CartViewModel>>(viewListDto);

                                ViewData["error"] = "";
                                if (detailsViewModel.Invoice)
                                {
                                    ViewData["error"] = "Please fill the fields above";
                                }


                                detailsViewModel.CartViewModelList.AddRange(viewListViewModel);
                                _cache.Set($"CartViewModel{userId}", viewListViewModel);
                            }
                            detailsViewModel.CartViewModelList = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
                            ViewData["id"] = 2;
                            return View(detailsViewModel);
                        }
                        if (detailsViewModel.Invoice && (string.IsNullOrEmpty(detailsViewModel.InvoiceCity) || string.IsNullOrEmpty(detailsViewModel.InvoiceAdress) || string.IsNullOrEmpty(detailsViewModel.InvoiceDDS) || string.IsNullOrEmpty(detailsViewModel.InvoiceEIK) || string.IsNullOrEmpty(detailsViewModel.InvoiceFirm) || string.IsNullOrEmpty(detailsViewModel.InvoiceMOL) || string.IsNullOrEmpty(detailsViewModel.InvoiceType)))
                        {
                            List<CartViewModel> viewListViewModel;
                            if (!_cache.TryGetValue($"CartViewModel{userId}", out viewListViewModel))
                            {
                                var viewListDto = __orderService.GetItemsInCart(userId);
                                viewListViewModel = _mapper.Map<List<CartViewModel>>(viewListDto);


                                detailsViewModel.CartViewModelList.AddRange(viewListViewModel);
                                detailsViewModel.Invoice = false;
                                _cache.Set($"CartViewModel{userId}", viewListViewModel);
                            }
                            ViewData["id"] = 2;
                            ViewData["error"] = "All Firm Fields must be filled";
                            detailsViewModel.CartViewModelList = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
                            return View(detailsViewModel);
                        }

                        if (!Request.Cookies.ContainsKey(cookie))
                        {
                            Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
                        }
                        ViewData["error"] = "";
                        var cookieId = Request.Cookies[cookie];


                        var userDetailsDto = _mapper.Map<UserDetailsDto>(detailsViewModel);
                        userDetailsDto.UsersessionId = userId;
                        userDetailsDto.Sum = __orderService.GetSumForSession(userId);



                        __orderService.CreateOrder(userDetailsDto);
                        _cache.Remove($"CartViewModel{userId}");
                        return View();
                    }
                    break;
                    
                default:
                    break;
            }
            return View();

        }



        // POST: Order/Delete/5
        [RequestFormLimits(MultipartBodyLengthLimit = 268435456)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CheckImg(IFormFile image)
        {
            // var userId = HttpContext.Request.Cookies["user_id"];
            if (image==null)
            {
                return BadRequest("Please attach a image file");
            }
            if (!image.ContentType.Contains("image"))
            {
                return BadRequest("Please attach a image file");
            }
            //  Image images = Image.FromStream(image.OpenReadStream(), true, true);
            using (MemoryStream ms = new MemoryStream(100))
            {
                image.OpenReadStream().CopyTo(ms);

                System.Drawing.Image images = System.Drawing.Image.FromStream(ms);

                if (images.Width < 700 || images.Height < 1400)
                {
                    return BadRequest("The photo doesn't meet the requirements");
                }


                string base64String = null;
                byte[] imageBytes = ms.ToArray();
                base64String = Convert.ToBase64String(imageBytes);

                return Ok(base64String);
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateImg([FromBody] ObjectsInCanvas objectsInCanvas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request 400");
            }

            var base64String = objectsInCanvas.Design.Split(",")[1];
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                var img = System.Drawing.Image.FromStream(ms, true);
                string cookie = "BeachTowelShop-Session";
                if (!Request.Cookies.ContainsKey(cookie))
                {
                  Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
                }
                if (Request.Cookies[cookie] != null)
                {

                    string cookieValueFromReq = Request.Cookies[cookie];

                   CreateProduct(cookieValueFromReq, img, objectsInCanvas);

                }











              
            }
            return Ok("Added to Cart");
        }

        private void CreateProduct(string cookieValueFromReq, Image img, ObjectsInCanvas objectsInCanvas)
        {
            string path = $"C:/Users/damot/source/repos/BeachTowelShop/BeachTowelShop-App/wwwroot/received/{cookieValueFromReq.Substring(Math.Max(0, cookieValueFromReq.Length - 10))}";
            OrderDataViewModel orderDataViewModel = new OrderDataViewModel();
            var fullPath = path;
            //NOT SURE IF WE WANT THAT
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                fullPath = $"{di}/{Guid.NewGuid()}.png";
                img.Save(fullPath, ImageFormat.Png);
            }
            else
            {
                fullPath = $"{path}/{Guid.NewGuid()}.png";
                img.Save(fullPath, ImageFormat.Png);

            }
            var splitPath = fullPath.Split("/").TakeLast(3).ToList();
            orderDataViewModel.SessionId = cookieValueFromReq;
            orderDataViewModel.DesignName = splitPath[2];
            orderDataViewModel.DesignFolderPath = splitPath[0] + "/" + splitPath[1];
            orderDataViewModel.Size = objectsInCanvas.Size;
            orderDataViewModel.Count = int.Parse(objectsInCanvas.Count);
            orderDataViewModel.Price = __productService.GetPriceForSize(orderDataViewModel.Size);
            orderDataViewModel.ProductId = splitPath[2];
            orderDataViewModel.Sum = orderDataViewModel.Price * orderDataViewModel.Count;
            // orderDataViewModel.SessionId = Request.Cookies[cookie];
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            var listTextOrderDataViewModel = new List<TextOrderDataViewModel>();
            using (JsonDocument document = JsonDocument.Parse(objectsInCanvas.Objects))
            {
                JsonElement root = document.RootElement;
                using (JsonWriter writer = new JsonTextWriter(sw))
                {

                    foreach (JsonElement obj in root.EnumerateArray())
                    {

                        if (obj.GetProperty("type").ToString() == "text")
                        {

                            writer.Formatting = Formatting.Indented;

                            writer.WriteStartObject();
                            writer.WritePropertyName("backgroundColor");
                            writer.WriteValue(obj.GetProperty("backgroundColor").ToString());
                            writer.WritePropertyName("fontWeight");
                            writer.WriteValue(obj.GetProperty("fontWeight").ToString());
                            writer.WritePropertyName("text");
                            writer.WriteValue(obj.GetProperty("text").ToString());
                            writer.WritePropertyName("fontWeight");
                            writer.WriteValue(obj.GetProperty("fontWeight").ToString());
                            writer.WritePropertyName("fontSize");
                            writer.WriteValue(obj.GetProperty("fontSize").ToString());
                            writer.WritePropertyName("stroke");
                            writer.WriteValue(obj.GetProperty("stroke").ToString());
                            writer.WritePropertyName("fill");
                            writer.WriteValue(obj.GetProperty("fill").ToString());
                            writer.WritePropertyName("fontFamily");
                            writer.WriteValue(obj.GetProperty("fontFamily").ToString());
                            writer.WritePropertyName("underline");
                            writer.WriteValue(obj.GetProperty("underline").ToString());
                            writer.WritePropertyName("overline");
                            writer.WriteValue(obj.GetProperty("overline").ToString());
                            writer.WritePropertyName("fontStyle");
                            writer.WriteValue(obj.GetProperty("fontStyle").ToString());
                            writer.WriteEndObject();
                            var textOrderDataViewModel = JsonConvert.DeserializeObject<TextOrderDataViewModel>(sb.ToString());
                            textOrderDataViewModel.UserSessionId = cookieValueFromReq;
                            listTextOrderDataViewModel.Add(textOrderDataViewModel);
                            sb.Clear();
                        }

                    }

                }
                var userSessionDto = _mapper.Map<UserSessionCartDto>(orderDataViewModel);
                var userTextList = _mapper.Map<List<UserTextSessionDto>>(listTextOrderDataViewModel);
                var cartItemDto = __orderService.SaveToCart(userSessionDto, userTextList);

                var userId = cookieValueFromReq;

                List<CartViewModel> itemsInCache;
                if (!_cache.TryGetValue($"CartViewModel{userId}", out itemsInCache))
                {
                    itemsInCache = new List<CartViewModel>();
                    _cache.Set($"CartViewModel{userId}", itemsInCache);
                }
                itemsInCache = _cache.Get($"CartViewModel{userId}") as List<CartViewModel>;
                var cartItem = _mapper.Map<CartViewModel>(cartItemDto);
                itemsInCache.Add(cartItem);
                _cache.Remove($"CartViewModel{userId}");
                _cache.Set($"CartViewModel{userId}", itemsInCache);

            }


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

    
