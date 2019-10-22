using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebAPI.Data;
using CoreWebAPI.Repositories;
using CoreWebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CoreWebAPI
{
    public class Startup
    {
        public static IConfiguration _configuration { get; private set; }

        //使用已经创建好的IConfiguration对象
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //IoC容器
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //添加自定义服务
            services.AddTransient<IMailService,LocalMailService>();
            //添加EFCore服务,默认情况下使用的是Scope生命周期,这里指定为transient
            services.AddDbContext<MyDbContext>(options=> {
                options.EnableSensitiveDataLogging(true);
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            },ServiceLifetime.Transient);
            //添加自定义仓储服务
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        //中间件
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory,MyDbContext myDbContext)
        {
            //中间件Nlog
            loggerFactory.AddNLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //正式环境中，遇到异常应当记录并捕获，使用下面中间件处理
                app.UseExceptionHandler();
            }
            //填充种子数据
            myDbContext.SeedForContext();
            //可以配置Status Code中间件
            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
