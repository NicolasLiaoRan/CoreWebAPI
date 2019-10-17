using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoreWebAPI
{
    public class Startup
    {
        //IoC容器
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        //中间件
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //正式环境中，遇到异常应当记录并捕获，使用下面中间件处理
                app.UseExceptionHandler();
            }
            //可以配置Status Code中间件
            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
