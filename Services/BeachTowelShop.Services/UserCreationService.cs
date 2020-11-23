using BeachTowelShop.Data;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeachTowelShop.Services
{
    public class UserCreationService 
    {
        public static async Task Initialize(ApplicationDbContext context,
                               UserManager<User> userManager,
                               RoleManager<IdentityRole> roleManager,
                               string role,string email,string password)
        {
            context.Database.EnsureCreated();

            if (await roleManager.FindByNameAsync(role) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
          

            if (await userManager.FindByNameAsync(email) == null)
            {
                var user = new User
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = "6902341234"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role);
                }
               
            }
         
            }
        }
    }

