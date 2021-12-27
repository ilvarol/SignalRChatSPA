using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRChatSPA.Abstract;
using SignalRChatSPA.Concrete;
using SignalRChatSPA.Data;
using SignalRChatSPA.Data.Abstract;
using SignalRChatSPA.Data.Concrete;
using SignalRChatSPA.Hubs;
using SignalRChatSPA.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChatSPA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<RedisServer>();
            services.AddSingleton<ICacheService, RedisCacheService>();

            services.AddControllersWithViews();
            services.AddSignalR();
            //services.AddSignalR(o => o.EnableDetailedErrors = true);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SqlConStr"].ToString());
            });

            services.AddScoped<IMessageLogRepository, MessageLogRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICacheService cache)
        {
            cache.Add("groups", new GroupSource());
            cache.Add("clients", new ClientSource());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
