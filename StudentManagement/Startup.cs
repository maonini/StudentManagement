using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class Startup
    {
        //readonly不可更改
        private readonly IConfiguration _configuration;
        //构造函数
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //添加MVC服务
            //services.AddMvc();
            //需要返回xml的格式需要添加服务
            services.AddMvc().AddXmlSerializerFormatters();

            services.AddSingleton<IStudentRepository, MockStudentRepository>();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        //{ 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) =>
            //{
            //    context.Response.ContentType = "text/plain;charset=utf-8";

            //    logger.LogInformation("M1:传入请求");
            //    await next();
            //    logger.LogInformation("M1:传出响应");
            //});

            //app.Use(async (context,next) =>
            //{
            //    logger.LogInformation("M2:传入请求");
            //    await next();
            //    logger.LogInformation("M2:传出响应");
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("M3:处理请求，并生成响应");

            //    logger.LogInformation("M3:处理请求，并生成响应");
            //});

            ////修改默认文件
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();

            ////清除默认
            //defaultFilesOptions.DefaultFileNames.Clear();
            ////添加默认
            //defaultFilesOptions.DefaultFileNames.Add("test.html");

            ////添加默认文件中间件  index.html index.htm default.html default.html,必须要注册在静态文件中间件之前
            //app.UseDefaultFiles(defaultFilesOptions);


            ////添加静态文件中间件
            //app.UseStaticFiles();

            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("test.html");

            ////拥有UseDefaultFiles，UseStaticFiles功能的中间件
            //app.UseFileServer(fileServerOptions);
            //------------------------------------------------------  3.1初始代码------------------------------------------------------            
            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        //进程名
            //        //var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            //        //var configVal = _configuration["MyKey"];

            //        await context.Response.WriteAsync("Hello world");
            //    });
            //});
            //------------------------------------------------------  结束------------------------------------------------------    

            app.UseStaticFiles();


            //添加MVC中间件
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                 );
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hosting Environment:" + env.EnvironmentName);
            });
        }
    }
}
