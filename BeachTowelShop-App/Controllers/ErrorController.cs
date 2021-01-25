using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BeachTowelShop_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeachTowelShop.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/404")]
        public async Task<IActionResult> Error404()
        {

          
            return View();
        }
        [Route("error/400")]
        public async Task<IActionResult> Error400()
        {


            return View();
        }

        [Route("error/{code:int}")]
        public async Task<IActionResult> Error(int code)
        {
            // handle different codes or just return the default error view
            var error = new ErrorViewModel();
            error.RequestId = code.ToString();
     
            return View(error);
        }
    }
}