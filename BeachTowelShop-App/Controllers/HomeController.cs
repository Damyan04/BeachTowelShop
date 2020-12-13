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
using Microsoft.Extensions.Caching.Memory;

namespace BeachTowelShop.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IProductService __productService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public HomeController( IProductService productService, IMapper mapper,IMemoryCache memoryCache)
        {
           
            __productService = productService;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [Route("/")]
        
        public IActionResult Index()
        {

           
           
            HomePageViewModel homePageViewModel;
           
            if (!_cache.TryGetValue("HomePageViewModel",out homePageViewModel))
            {


                homePageViewModel = new HomePageViewModel();

                  var towelsDtoList = __productService.GetSizes();
            var towelViewModelList = _mapper.Map < List < HomePageTowelsViewModel >> (towelsDtoList);

            homePageViewModel.Towels.AddRange(towelViewModelList.OrderBy(a => a.Price));
                var generalCommentsDtoList=__productService.GetGeneralComments();
                var generalCommetsViewModelList=_mapper.Map<List<CommentViewModel>>(generalCommentsDtoList);
                homePageViewModel.Comments.AddRange(generalCommetsViewModelList);
                _cache.Set("HomePageViewModel", homePageViewModel);
            }
            homePageViewModel = _cache.Get("HomePageViewModel") as HomePageViewModel;
         

            
            
           
        
           
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
        
    }
}
