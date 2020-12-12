using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeachTowelShop.Areas.Admin.Models;
using BeachTowelShop.Models.Login;
using BeachTowelShop.Models.Orders;
using BeachTowelShop.Models.Products;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using BeachTowelShop_App.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BeachTowelShop.Areas.Admin
{

    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService __productService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        private readonly IAdminService __adminService;
        public AdminController(IProductService productService, IMapper mapper, IAdminService adminService, IMemoryCache memoryCache)
        {
            __productService = productService;
            _mapper = mapper;
            __adminService = adminService;
            _cache = memoryCache;
            // __orderService = orderService;
        }

        // GET: Admin
        [Route("Admin/Index")]
        [Authorize]
        public ActionResult Index()
        {
            // add login controler


            return View();
        }
        [Authorize]
        [Route("Admin/Orders")]
        public ActionResult Orders()
        {
            // add login controler
            var allOrdersDtoList = __adminService.GetAllOrders().OrderByDescending(a => a.Status);
            var orderDtoList = _mapper.Map<List<OrderViewModel>>(allOrdersDtoList);
            return View(orderDtoList);
        }
        [Authorize]
        [HttpGet]
        [Route("Admin/SizeAndPrice")]
        public ActionResult SizeAndPrice()
        {
            var allSizes = __productService.GetAllSizes();
            var allSizesDto = _mapper.Map<List<AdminSizesViewModel>>(allSizes);


            return View(allSizesDto);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveInDb(IFormCollection keyValuePairs)
        {
            if (keyValuePairs == null)
            {
                return RedirectToAction("SizeAndPrice");
            }
            //TODO: make it better
            var adminSizesViewModel = new AdminSizesViewModel() { Id = keyValuePairs["item.Id"], Name = keyValuePairs["item.Name"], Price = Double.Parse(keyValuePairs["item.Price"]) };
            if (!ModelState.IsValid)
            {
                return RedirectToAction("SizeAndPrice");
            }
            var sizesDto = _mapper.Map<SizeDto>(adminSizesViewModel);
            __adminService.SaveSizesToDb(sizesDto);
            _cache.Remove("HomePageViewModel");
            _cache.Remove("OrderProductViewModel");
            return RedirectToAction("SizeAndPrice");
        }
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveAdminProductInDb(IFormCollection keyValuePairs, string productId)
        {
            if (keyValuePairs == null)
            {
                return RedirectToAction("Item");
            }
            //TODO: make it better
            var adminSizesViewModel = new AdminSizesViewModel() { Id = keyValuePairs["item.Id"], Name = keyValuePairs["item.Name"], Price = Double.Parse(keyValuePairs["item.Price"]) };
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Item");
            }
            var sizesDto = _mapper.Map<SizeWithPriceDto>(adminSizesViewModel);
            __adminService.SaveAdminProductSize(sizesDto, productId);
          
            return RedirectToAction("Item");
        }
        [Authorize]
        [Route("Admin/UploadItem")]
        public ActionResult UploadItem()
        {
            // add login controler
            
            var allProductsDto = __adminService.GetAllProductsForAdmin();
            var allProductsViewModel = _mapper.Map<List<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>>(allProductsDto);

            return View(allProductsViewModel);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateItem(List<IFormFile> pic, string name, string productDescription, string categories)
        {
           
            if (categories == null)
            {
                var allProductsDto1 = __adminService.GetAllProductsForAdmin();
                var allProductsViewModel1 = _mapper.Map<List<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>>(allProductsDto1);
                allProductsViewModel1[0].Error = "fill in a category";
                return View(allProductsViewModel1);
            }
            var categoriesList = categories.Trim().Split(";",StringSplitOptions.RemoveEmptyEntries).ToList();
            var categoryDtoList = new List<CategoryDto>();
            for (int i = 0; i < categoriesList.Count; i++)
            {
                var categoryDto = new CategoryDto() { Name = categoriesList[i] };
                categoryDtoList.Add(categoryDto);
            }
            var picturePath = $"C:/Users/damot/source/repos/BeachTowelShop/BeachTowelShop-App/wwwroot/pictures";

            var picturePathList = new List<PictureDto>();
            if (pic == null)
            {
                var allProductsDto1 = __adminService.GetAllProductsForAdmin();
                var allProductsViewModel1 = _mapper.Map<List<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>>(allProductsDto1);
                allProductsViewModel1[0].Error = "attach a at lest one picture";
                return View(allProductsViewModel1);
               
            }
            foreach (var image in pic)
            {
                var picName = $"{name}-{Guid.NewGuid()}.png";
                var fullPath = $"{picturePath}/{picName}";

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
                        var allProductsDto1 = __adminService.GetAllProductsForAdmin();
                        var allProductsViewModel1 = _mapper.Map< List<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel >> (allProductsDto1);
                        allProductsViewModel1[0].Error = "The photo doesn't meet the requirements";
                        return View(allProductsViewModel1);
                      
                    }





                    images.Save(fullPath, ImageFormat.Png);
                    var pictureDto = new PictureDto() { Path="pictures/"+picName};
                    picturePathList.Add(pictureDto);

                }
            }
            var productDto = new AdminProductDto() { Name = name, Description = productDescription, CategoryViews=categoryDtoList, PictureList = picturePathList };
            __adminService.CreateProduct(productDto);
            var allProductsDto = __adminService.GetAllProductsForAdmin();
            var allProductsViewModel = _mapper.Map<List<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>>(allProductsDto);
            _cache.Remove("GalleryProductsViewModel");
            _cache.Remove("GalleryProductsViewModelFilter");
           
           
            
            return View("UploadItem",allProductsViewModel);
        }
        // GET: Admin/Details/5

        [Authorize]
        [Route("Admin/Info")]
        public ActionResult Info(string id)
        {
            // add login controler
            var fullOrderDto = __adminService.GetOrderById(id);
            var fullOrderViewModel = _mapper.Map<FullOrderViewModel>(fullOrderDto);
            var disignViewModelList = _mapper.Map<List<CartViewModel>>(fullOrderDto.FinishedDesings);
            var textViewModelList = _mapper.Map<List<TextOrderDataViewModel>>(fullOrderDto.TextForDesings);
            fullOrderViewModel.ListOfProducts.AddRange(disignViewModelList);
            fullOrderViewModel.TextOrderDataViews.AddRange(textViewModelList);
            return View(fullOrderViewModel);
           //TODO:Dising imgs can be a list

    }
        [Authorize]
        [Route("Admin/Item")]
        public ActionResult Item(string id)
        {
            var productDto = __adminService.GetAdminProduct(id);
            var productViewModel = _mapper.Map<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>(productDto);

            // add login controler
            return View(productViewModel);
        }

    }
}