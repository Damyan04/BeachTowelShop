using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeachTowelShop.Areas.Admin.Models;
using BeachTowelShop.Models.Login;
using BeachTowelShop.Models.Orders;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeachTowelShop.Areas.Admin
{

    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService __productService;
        private readonly IMapper _mapper;

        
        private readonly IAdminService __adminService;
        public AdminController(IProductService productService, IMapper mapper, IAdminService adminService)
        {
            __productService = productService;
            _mapper = mapper;
            __adminService = adminService;
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
            var allOrdersDtoList = __adminService.GetAllOrders().OrderByDescending(a=>a.Status);
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
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult SaveInDb(IFormCollection keyValuePairs)
        {
            if (keyValuePairs == null)
            {
                return RedirectToAction("SizeAndPrice");
            }
            //TODO: make it better
            var adminSizesViewModel = new AdminSizesViewModel() { Id=keyValuePairs["item.Id"],Name = keyValuePairs["item.Name"], Price = Double.Parse(keyValuePairs["item.Price"]) };
            if (!ModelState.IsValid)
            {
                return RedirectToAction("SizeAndPrice");
            }
          var sizesDto=  _mapper.Map<SizeDto>(adminSizesViewModel);
            __adminService.SaveSizesToDb(sizesDto);
           return RedirectToAction("SizeAndPrice");
        }

        [Authorize]
        [Route("Admin/UploadItem")]
        public ActionResult UploadItem()
        {
            // add login controler


            return View();
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
        public ActionResult ItemInfo(string id)
        {
            // add login controler
            return View();
        }

    }
}