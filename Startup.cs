using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentity.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreIdentity
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;//sayı zorunluluğu
                options.Password.RequireLowercase = true;//küçük harf
                options.Password.RequiredLength = 8;//minimum 8 karakter
                options.Password.RequireNonAlphanumeric = true;//alfanümerik olması
                options.Password.RequireUppercase = true;//büyük harf zorunluluğu

                options.Lockout.MaxFailedAccessAttempts = 5;//max hatalı giriş sayısı
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//kullanıcı ne kadar süre boyunca sisteme giriş yapamasın
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/administrator/login";
                options.LogoutPath = "/administrator/log-out";
                options.AccessDeniedPath = "/administrator/access-denied";
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".AspNetCoreIdentity",
                    Path = "/",
                    SameSite = SameSiteMode.Strict
                };
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
