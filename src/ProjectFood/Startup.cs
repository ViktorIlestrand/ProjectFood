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

namespace ProjectFood
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = @"Server=tcp:patodb.database.windows.net,1433;Initial Catalog=PatoDB;Persist Security Info=False;User ID=PatoDBAdmin;Password=Sommar2016!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddDbContext<IdentityDbContext>(
                options => options.UseSqlServer(connString));

            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Cookies.ApplicationCookie.LoginPath = "/user/login";
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseIdentity();
            app.UseMvcWithDefaultRoute();
        }
    }
}
