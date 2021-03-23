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
        //readonly���ɸ���
        private readonly IConfiguration _configuration;
        //���캯��
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //���MVC����
            //services.AddMvc();
            //��Ҫ����xml�ĸ�ʽ��Ҫ��ӷ���
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

            //    logger.LogInformation("M1:��������");
            //    await next();
            //    logger.LogInformation("M1:������Ӧ");
            //});

            //app.Use(async (context,next) =>
            //{
            //    logger.LogInformation("M2:��������");
            //    await next();
            //    logger.LogInformation("M2:������Ӧ");
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("M3:�������󣬲�������Ӧ");

            //    logger.LogInformation("M3:�������󣬲�������Ӧ");
            //});

            ////�޸�Ĭ���ļ�
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();

            ////���Ĭ��
            //defaultFilesOptions.DefaultFileNames.Clear();
            ////���Ĭ��
            //defaultFilesOptions.DefaultFileNames.Add("test.html");

            ////���Ĭ���ļ��м��  index.html index.htm default.html default.html,����Ҫע���ھ�̬�ļ��м��֮ǰ
            //app.UseDefaultFiles(defaultFilesOptions);


            ////��Ӿ�̬�ļ��м��
            //app.UseStaticFiles();

            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("test.html");

            ////ӵ��UseDefaultFiles��UseStaticFiles���ܵ��м��
            //app.UseFileServer(fileServerOptions);
            //------------------------------------------------------  3.1��ʼ����------------------------------------------------------            
            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        //������
            //        //var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            //        //var configVal = _configuration["MyKey"];

            //        await context.Response.WriteAsync("Hello world");
            //    });
            //});
            //------------------------------------------------------  ����------------------------------------------------------    

            app.UseStaticFiles();


            //���MVC�м��
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
