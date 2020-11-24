﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BeachTowelShop.Models;
using BeachTowelShop.Models.Products;
using BeachTowelShop.Models.HomePage;
using BeachTowelShop.Models.Comments;
using Microsoft.AspNetCore.Http;
using BeachTowelShop.Services.Interfaces;
using AutoMapper;
using BeachTowelShop_App.Models;

namespace BeachTowelShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService __productService;
        private readonly IMapper _mapper;
        public HomeController(ILogger<HomeController> logger, IProductService productService, IMapper mapper)
        {
            _logger = logger;
            __productService = productService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            bool admin = User.IsInRole("Admin");
            if (isAuthenticated&&admin)
            {
                return View("~/Areas/Admin/Views/Admin/Index.cshtml");
            }
            var homePageViewModel = new HomePageViewModel();
          

            var towelsDtoList = __productService.GetSizes();
            var towelViewModelList = _mapper.Map < List < HomePageTowelsViewModel >> (towelsDtoList);

            homePageViewModel.Towels.AddRange(towelViewModelList.OrderBy(a => a.Price));
            var generalCommentsDtoList=__productService.GetGeneralComments();
            var generalCommetsViewModelList=_mapper.Map<List<CommentViewModel>>(generalCommentsDtoList);
            homePageViewModel.Comments.AddRange(generalCommetsViewModelList);

            string cookie = "BeachTowelShop-Session";
            if (!Request.Cookies.ContainsKey(cookie))
            {
                Set("BeachTowelShop-Session", Guid.NewGuid().ToString(), 100);
            }
            
           
        
           
            return View(homePageViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
