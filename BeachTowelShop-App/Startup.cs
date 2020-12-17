using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using BeachTowelShop.Automapper;
using BeachTowelShop.Data;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services;
using BeachTowelShop.Services.Automapper;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BeachTowelShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("ApplicationDbContext"), b => b.MigrationsAssembly("BeachTowelShop.Data")));
            services.AddIdentity<BeachTowelShop.Data.Models.User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders().AddDefaultUI();
            services.AddMvc()
    .AddRazorPagesOptions(options =>
    {
           // options.AllowAreas = true;
            options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
        options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
    });

            services.ConfigureApplicationCookie(options =>
            {
                
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            
            services.AddAutoMapper(typeof(CategoryDtosProfile).Assembly, typeof(PictureDtosProfile).Assembly, typeof(ProductDtosProfile).Assembly, typeof(SizeDtosProfile).Assembly,typeof(CategoryViewModelProfile).Assembly);
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            services.AddRazorPages();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddScoped<IEmailService, EmailService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //  services.AddDistributedMemoryCache();


            services.AddSession(options =>
            {
                options.Cookie.Name = "BeachTowelShop-Session";

                options.Cookie.IsEssential = true;
                
                options.Cookie.SameSite = SameSiteMode.None;
             



            });
           services.AddMemoryCache();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            // services.AddTransient(typeof(ISeeder), typeof(Seeder));
            //services.AddScoped(ISeeder, Seeder);
            //services.AddSingleton<IEmailService, EmailSender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ApplicationDbContext context, RoleManager<IdentityRole> role, UserManager<User> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
        
            //var dataTextProduct = System.IO.File.ReadAllText(@"product.json");
            //var dataTextPicture = System.IO.File.ReadAllText(@"pictures.json");
            //var dataTextCategory = System.IO.File.ReadAllText(@"categories.json");
            //var dataTextSize = System.IO.File.ReadAllText(@"sizes.json");
            //Seeder.Seedit(dataTextProduct,dataTextPicture,dataTextSize,dataTextCategory, app.ApplicationServices);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           app.UseCookiePolicy();
          
            app.UseSession();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
        name: "Identity",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
            
        }
    }
}
