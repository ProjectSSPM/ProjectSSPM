﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSMP.Models;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using NToastNotify;

namespace SSMP
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
            //var connection = @"Server=192.168.88.100;Initial Catalog=sspm;Integrated Security=False;User ID=sspm;Password=sspm@dev;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var connection = @"Server=den1.mssql5.gear.host;Initial Catalog=sspm;Integrated Security=False;User ID=sspm;Password=Gi90MMTY!H_i;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<sspmContext>(options => options.UseSqlServer(connection));
            services.AddMvc()
                //.AddSessionStateTempDataProvider()
                .AddNToastNotify();

            //services.AddSession();
            services.AddAuthentication("FiverSecurityScheme")
                 .AddCookie("FiverSecurityScheme", options =>
                 {
                     options.AccessDeniedPath = new PathString("/Account/Access");
                     options.LoginPath = new PathString("/Security/Login");
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                 });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseSession();
            app.UseAuthentication();
            //app.UseMvcWithDefaultRoute();
            
           
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseNToastNotify();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Security}/{action=Login}/{id?}");
            });
        }
    }
}
