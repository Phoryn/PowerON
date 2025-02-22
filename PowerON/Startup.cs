﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using PowerON.DAL;
using PowerON.Infrastructure;
using PowerON.Models;

namespace PowerON
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("StoreDatabase")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddDefaultTokenProviders()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<StoreContext>();
            
            

            services.AddMemoryCache();


            services.AddAuthentication().
                AddFacebook(options =>
            {
                options.AppId = "637396096744159";
                options.AppSecret = "99b00bc0e2f913a82214ff0c3e16bc94";
                //options.Scope.Add("public_profile");
                //options.Fields.Add("name");
                //options.AuthorizationEndpoint = "Facebook";

            });
            
            //session
            services.AddDistributedMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".PowerOn";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = false;

            });
            services.AddHttpContextAccessor();
            //session

            services.AddMvc(options => {
                options.SslPort = 44300;
                options.Filters.Add(new RequireHttpsAttribute());
            }
).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            IServiceProvider service,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            

            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            


            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "StaticPages",
                    template: "strony/{viewname}.html",
                    defaults: new { controller = "Home", action = "StaticContent" }
                    );

                routes.MapRoute(
                    name: "ProductDetails",
                    template: "album-{id}.html",
                    defaults: new { controller = "Store", action = "Details"}
                    );

                routes.MapRoute(
                    name: "ProductList",
                    template: "gatunki/{genrename}",
                    defaults: new { controller = "Store", action = "List" },
                    constraints: new { genrename = @"[\w& ]+"}
                    );



                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
            ApplicationRole.CreateUserRoles(service).Wait();
        }


    }
}
