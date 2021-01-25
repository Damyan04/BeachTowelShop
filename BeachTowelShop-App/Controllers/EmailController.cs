using BeachTowelShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeachTowelShop_App.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
      
        [HttpPost]
        public async Task<IActionResult> SendDiscounts(string email)
        {
            if (email != null)
            {
               await _emailService.SendEmail(email, "Discount", "Hey,here is your discount").ConfigureAwait(false);
            }
             
            return RedirectToAction("Index","Home");
        }
       
    }
}
