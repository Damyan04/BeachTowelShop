using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeachTowelShop.Areas.Admin.Models;

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
    [Authorize(Roles = "admins")]
    public class AdminController : Controller
    {
        private readonly IProductService __productService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;

        private readonly IAdminService __adminService;
        public AdminController(IProductService productService, IMapper mapper, IAdminService adminService, IMemoryCache memoryCache, IEmailService emailService)
        {
            __productService = productService;
            _mapper = mapper;
            __adminService = adminService;
            _cache = memoryCache;
            _emailService = emailService;
            // __orderService = orderService;
        }

        // GET: Admin
        [Route("Admin/Index")]
        [Authorize]
        public IActionResult Index()
        {
            // add login controler


            return View();
        }
        [Authorize(Roles = "admins")]
        [Route("Admin/Orders/{startPage?}")]
        public IActionResult Orders(int startPage = 0)
        {
            var pageSize = 20;

            var fromPage = startPage * pageSize;

            ViewBag.NextPage = startPage + 1;
            ViewBag.PreviousPage = startPage - 1;

            // add login controler

            List<OrderViewModel> orderDtoList;
            if(!_cache.TryGetValue("AdminOrderViewModel", out orderDtoList))
            {
                var allOrdersDtoList = __adminService.GetAllOrders().OrderByDescending(a => a.Status);
                orderDtoList = _mapper.Map<List<OrderViewModel>>(allOrdersDtoList);
                _cache.Set("AdminOrderViewModel", orderDtoList);
            }
            orderDtoList=_cache.Get("AdminOrderViewModel") as List<OrderViewModel>;
            var lastPage = orderDtoList.Count / pageSize;
            if (fromPage >= lastPage)
            {
                ViewBag.NextPage = lastPage;
               
            }
            orderDtoList = orderDtoList.Skip(fromPage).Take(pageSize).ToList();
            return View(orderDtoList);
        }
        [Authorize(Roles = "admins")]
        [HttpGet]
        [Route("Admin/SizeAndPrice")]
        public IActionResult SizeAndPrice()
        {
            var allSizes = __productService.GetAllSizes();
            var allSizesDto = _mapper.Map<List<AdminSizesViewModel>>(allSizes);


            return View(allSizesDto);
        }

        [Authorize(Roles = "admins")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SaveInDb(IFormCollection keyValuePairs)
        {
            if (keyValuePairs == null)
            {
                return RedirectToAction("SizeAndPrice");
            }
            double count = 0;
            var canConvert = double.TryParse(keyValuePairs["item.Price"], out count);
            AdminSizesViewModel adminSizesViewModel  =new AdminSizesViewModel();
            if (canConvert ||( double.Parse(keyValuePairs["item.Price"]) > 0))
            {
                adminSizesViewModel.Id = keyValuePairs["item.Id"];
                adminSizesViewModel.Name = keyValuePairs["item.Name"];
                adminSizesViewModel.Price = Double.Parse(keyValuePairs["item.Price"]);
            }
            //TODO: make it better

          
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var sizesDto = _mapper.Map<SizeDto>(adminSizesViewModel);
            __adminService.SaveSizesToDb(sizesDto);
            _cache.Remove("HomePageViewModel");
            _cache.Remove("OrderProductViewModel");
            return RedirectToAction("SizeAndPrice");
        }

        [Authorize(Roles = "admins")]
        [Route("Admin/UploadItem")]
        public IActionResult UploadItem()
        {
            // add login controler
            
            var allProductsDto = __adminService.GetAllProductsForAdmin();
            var allProductsViewModel = _mapper.Map<List<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>>(allProductsDto);

            return View(allProductsViewModel);
        }
        [Authorize(Roles = "admins")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreateItem(List<IFormFile> pic, string name, string productDescription, string categories)
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
            var picturePathList = new List<PictureDto>();
            CreateImg(pic,name,picturePathList);
           var productDto = new AdminProductDto() { Name = name, Description = productDescription, CategoryViews = categoryDtoList, PictureList = picturePathList };
            __adminService.CreateProduct(productDto);
            var allProductsDto = __adminService.GetAllProductsForAdmin();
            var allProductsViewModel = _mapper.Map<List<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>>(allProductsDto);
            _cache.Remove("GalleryProductViewModel");
            _cache.Remove("CategoryViewModel");
            //_cache.Remove("HomePageViewModel");
           
            _cache.Remove("OrderProductViewModel");
           
           



            return View("UploadItem",allProductsViewModel);
        }
        // GET: Admin/Details/5

        [Authorize(Roles = "admins")]
        [Route("Admin/Info")]
        public IActionResult Info(string id)
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
        [Authorize(Roles = "admins")]
        [Route("Admin/Item")]
        public IActionResult Item( string itemId)
        {
            var productDto = __adminService.GetAdminProduct(itemId);
            var productViewModel = _mapper.Map<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>(productDto);
            productViewModel.Id = itemId;
            // add login controler
            return View(productViewModel);
        }
        [Authorize(Roles = "admins")]
        [Route("Admin/Item")]
        [HttpPost]
        public IActionResult UpdateItem(AdminProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           
            __adminService.UpdateItem(productDto);
            string id = productDto.Id;
            var updatedProductDto=  __adminService.GetAdminProduct(id);
            var updatedProductViewModel = _mapper.Map<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>(updatedProductDto);
            _cache.Remove("GalleryProductViewModel");
            _cache.Remove("CategoryViewModel");
            return View("Item", updatedProductViewModel);
        }
        //AddPicAndCat
        [Authorize(Roles = "admins")]
        [Route("Admin/AddPicAndCat")]
        [HttpPost]
        public IActionResult AddPicAndCat(List<IFormFile> pic, string category,string name,string id)
        {
            List<string> categoryList=new List<string>();
            if (category != null)
            {
                 categoryList = category.Trim().Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            var picturePathList = new List<PictureDto>();
            CreateImg(pic, name, picturePathList);
           
            __adminService.CreateCategoryAndPictureForItem(picturePathList,categoryList,id);
            var updatedProductDto = __adminService.GetAdminProduct(id);
            var updatedProductViewModel = _mapper.Map<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>(updatedProductDto);
            _cache.Remove("GalleryProductViewModel");
            _cache.Remove("CategoryViewModel");
            return View("Item", updatedProductViewModel);
        }
        [Authorize(Roles = "admins")]
        [Route("Admin/DeletePath")]
        [HttpPost]
        public IActionResult DeletePath(string pathId,string id)
        {
            if (pathId == null || id == null)
            {
                return BadRequest();
            }
            __adminService.DeletePicture(id, pathId);
            var updatedProductDto = __adminService.GetAdminProduct(id);
            var updatedProductViewModel = _mapper.Map<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>(updatedProductDto);
            _cache.Remove("GalleryProductViewModel");
            _cache.Remove("CategoryViewModel");
            return View("Item", updatedProductViewModel);
        }
        
           
        [Route("Admin/DeleteCategory")]
        [HttpPost]
        [Authorize(Roles = "admins")]
        public IActionResult DeleteCategory(string id, string categoryId)
        {
            if (categoryId == null || id == null)
            {
                return BadRequest();
            }
            __adminService.DeleteCategory(id, categoryId);
            var updatedProductDto = __adminService.GetAdminProduct(id);
            var updatedProductViewModel = _mapper.Map<BeachTowelShop_App.Areas.Admin.Models.ProductViewModel>(updatedProductDto);
            _cache.Remove("GalleryProductViewModel");
            _cache.Remove("CategoryViewModel");
            return View("Item", updatedProductViewModel);
        }
        [Authorize(Roles = "admins")]
        [Route("Admin/DeleteItem")]
        [HttpPost]
        public IActionResult DeleteItem(string id)
        {
            if (id==null)
            {
                return BadRequest();
            }
            
            __adminService.DeleteItemById(id);


            _cache.Remove("GalleryProductViewModel");
            _cache.Remove("CategoryViewModel");
            return RedirectToAction("UploadItem");
        }
        [Authorize(Roles = "admins")]
        [HttpPost]
        public IActionResult ChangeStatus(string status,string id,string email)
        {
            if (status != null && id != null)
            {
            var statusName= (Status)Enum.Parse(typeof(Status), status);
            status = statusName.ToString();
            __adminService.ChangeStatus(id, status);
                
            }
            if (email != null)
            {
                _emailService.SendEmail(email, "Order status", $"Hey,the status of your order is {status}");
            }
            var fullOrderDto = __adminService.GetOrderById(id);
            var fullOrderViewModel = _mapper.Map<FullOrderViewModel>(fullOrderDto);
            var disignViewModelList = _mapper.Map<List<CartViewModel>>(fullOrderDto.FinishedDesings);
            var textViewModelList = _mapper.Map<List<TextOrderDataViewModel>>(fullOrderDto.TextForDesings);
            fullOrderViewModel.ListOfProducts.AddRange(disignViewModelList);
            fullOrderViewModel.TextOrderDataViews.AddRange(textViewModelList);
            _cache.Remove("AdminOrderViewModel");
            var allOrdersDtoList = __adminService.GetAllOrders().OrderByDescending(a => a.Status);
           var orderDtoList = _mapper.Map<List<OrderViewModel>>(allOrdersDtoList);
            _cache.Set("AdminOrderViewModel", orderDtoList);
            return View("Info", fullOrderViewModel);
        }

        private IActionResult CreateImg(List<IFormFile> pic,string name, List<PictureDto> picturePathList)
        {
            var picturePath = $"C:/Users/damot/source/repos/BeachTowelShop/BeachTowelShop-App/wwwroot/pictures";

            //var picturePathList = new List<PictureDto>();
            if (pic == null)
            {
                return BadRequest();

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

                    //if (images.Width < 700 || images.Height < 1400)
                    //{
                    //    return BadRequest("Attach a better photo");

                    //}





                    images.Save(fullPath, ImageFormat.Png);
                    var pictureDto = new PictureDto() { Path = "pictures/" + picName };
                    picturePathList.Add(pictureDto);

                }
            }
            return Ok();
        }
       

    }
}