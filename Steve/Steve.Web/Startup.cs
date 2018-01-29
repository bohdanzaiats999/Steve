using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steve.BLL.Interfaces;
using Steve.BLL.Services;
using Steve.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace Steve.Web
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
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGoodsService, GoodsService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICartService, CartService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(null,
                "",
                new
                {
                    controller = "Goods",
                    action = "GoodsList",
                    category = (string)null,
                    page = 1
                }
            );
                routes.MapRoute(
                    name: null,
                    template: "Page{page}",
                    defaults: new { controller = "Goods", action = "GoodsList", category = (string)null },
                    constraints: new { page = @"\d+" }
            );

                routes.MapRoute(null,
                    "{category}",
                    new { controller = "Goods", action = "GoodsList", page = 1 }
            );

                routes.MapRoute(null,
                     "{category}/Page{page}",
                    new { controller = "Goods", action = "GoodsList" },
                    new { page = @"\d+" });
            });
        }
    }
}
