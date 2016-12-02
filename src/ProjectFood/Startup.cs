using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ProjectFood.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;
using ProjectFood.Models.ViewModels.UserVM;
using Microsoft.Extensions.Configuration;

namespace ProjectFood
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);

            if(env.IsDevelopment())
                builder.AddUserSecrets();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();     
        }
        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {

            var connString = Configuration["connString"];

            services.AddDbContext<PatoDBContext>(
                options => options.UseSqlServer(connString));

            services.AddDbContext<IdentityDbContext>(
                options => options.UseSqlServer(connString));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Cookies.ApplicationCookie.LoginPath = "/user/login";
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            services.AddSession();
            services.AddMemoryCache();

            Mapper.Initialize((config) =>
            {
                config.CreateMap<UserFoodItem, UserFoodItemVM>();
            });

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSession();
            app.UseDeveloperExceptionPage();
            app.UseIdentity();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            
        }
    }
}
