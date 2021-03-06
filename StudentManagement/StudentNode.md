# 2021-03-16
## 指定目标框架
>       <TargetFramework>netcoreapp3.1</TargetFramework>
## 指定应用程序的托管形式
>       <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>

+		由于项目发布之后要运行在某一个环境之中	所以选择InProcess还是OutOfProcess（进程内/进程外），默认为OutOfProcess
+		InProcess的值指定我们想要使用进程内托管模型，即在IIS工作进程（w3wp.exe）中托管我们的ASP.NET Core应用程序
+		OutOfProcess的值指定我们要使用进程外托管模型，即将Web请求转发到后端的ASP.NET Core中，而整个应用程序是运行在ASP.NET Core中内置的Kestrel中
## 包的引用
>	<PackageReference Include="">
```
    eg:
        <ItemGroup>
			<PackageReference Include="Microsoft.AspNetCore.App">
			<PackageReference Include="Microsoft.Razor.Design" Version="2.2.0" PrivateAssets="All">
		</ItemGroup>
```

## 获取当前进程名
`	System.Diagnostics.Process.GetCurrentProcess().ProcessName;`

## 申明构造函数
>	输入ctor 按两下TAB键


## ASP.NET Core 中的配置源
* appsettings.json文件
	* 读取顺序
	    * appsetting.json,appsettings.{Environment}.json,不同环境下对应不同的托管环境
		* User secrets(用户机密)
		* 右键点击项目选择管理用户机密，在secrers.json内对配置参数赋值
		* Environment variables(环境变量)
		* Command-line arguments(命令行参数)
		    * 在命令行对配置赋值dotnet run Mykey="value from command line"
		* 优先级为
		    * Command-line arguments>User secrets>Environment variables>appsetting.json>appsettings.{Environment}.json

* IConfiguration获取配置，配置接口



# 2021-03-17
## ASP.NET Core 的中间件
*	内置中间件
> Logging(日志)->StaticFiles(文件处理)->MVC
*	可同时被访问请求
*	可以处理请求后，然后将请求传递给下一个中间件。
*	可以处理请求后，并使用管道短路。
*	可以处理传出响应。
*	中间件是按照添加的顺序执行的。

*	设置中间件字符编码
>		context.Response.ContentType = "text/plain;charset=utf-8";
	
*	中间件写法
```
	eg:
		app.Use(async (context, next) =>
            {
                context.Response.ContentType = "text/plain;charset=utf-8";

                await context.Response.WriteAsync("Use 中间件");
                await next();
            });

		app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Run 中间件");
            });
```

*	内容传递到下一个中间件
>		await next();
	
*	日志记录
>		ILogger<Startup> logger
```
		eg:
		     public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.Use(async (context, next) =>
                {
                    context.Response.ContentType = "text/plain;charset=utf-8";

                    logger.LogInformation("M1:传入请求");
                    await next();
                    logger.LogInformation("M1:传出响应");
                });
               
                app.Use(async (context,next) =>
                {
                    logger.LogInformation("M2:传入请求");
                    await next();
                    logger.LogInformation("M2:传出响应");
                });

                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("M3:处理请求，并生成响应");

                    logger.LogInformation("M3:处理请求，并生成响应");
                });

                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", async context =>
                    {
                        //进程名
                        //var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

                        //var configVal = _configuration["MyKey"];

                        await context.Response.WriteAsync("Hello world");
                    });
                });
            }
```
>			输出结果：
>>              web:
>>>					M3:处理请求，并生成响应
>>				控台：
>>>					StudentManagement.Startup: Information: M1:传入请求
>>>					StudentManagement.Startup: Information: M2:传入请求
>>>					StudentManagement.Startup: Information: M3:处理请求，并生成响应
>>>					StudentManagement.Startup: Information: M2:传出响应
>>>					StudentManagement.Startup: Information: M1:传出响应
>>			M3为终端中间件

* 控台日志
>			logger.LogInformation("M1:传入请求");

*	所有的请求处理都会在每个中间件组件调用next()方法之前触发。请求按照M1->M2->M3,依次穿过所有管道
*	当中间件处理请求并产生响应时，请求流程会在管道开始反向逆转传递。
*	所有的响应处理都会在没哥哥中间件组件调用next()方法之前触发。响应按照M3->M2->M1，依次穿过所有管道

## 添加静态文件
* 创建wwwroot文件夹
* 添加静态文件中间件
  * 访问wwwroot下的静态文件
   > app.UseStaticFiles();
* 添加默认文件中间件  
  * （index.html index.htm default.html default.html,必须要注册在静态文件中间件之前）
    > app.UseDefaultFiles();
* 修改默认文件
```
		eg:
			//修改默认文件
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();

            //清除默认
            defaultFilesOptions.DefaultFileNames.Clear();
            //添加默认
            defaultFilesOptions.DefaultFileNames.Add("test.html");

            //添加默认文件中间件  index.html index.htm default.html default.html,必须要注册在静态文件中间件之前
            app.UseDefaultFiles(defaultFilesOptions);
```
* 同时拥有UseDefaultFiles，UseStaticFiles，UseDirectoryBrowser功能的中间件
> **实际生产项目中不推荐使用，会暴露项目目录**
```	
	eg:
        FileServerOptions fileServerOptions = new FileServerOptions();
        fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
        fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("test.html");

        //拥有UseDefaultFiles，UseStaticFiles功能的中间件
        app.UseFileServer(fileServerOptions);
```
# 2021-03-18
## 抛出异常
> throw new Exception("您的请求在管道中发生了一些错误，请检查");

* 在管道中要使用UseDeveloperExceptionPage 启用中间件
```
    eg:
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
```
* 必须竟可能的在管道中提早注入，因为要在其他中间件之前拦截到异常

* 异常展示包含Stack Trace,Query String Cookies和HTTP Headers

* 用于自定义异常页面，可以使用DeveloperExceptionPageOptions对象

## 开发环境变量
* Development(开发环境)，Staging(演示环境、预发布环境)，Production(正式环境)

* ASPNETCORE_ENVIRONMENT变量可以设置在运行时环境（Runtime Environment） 

* env.EnvironmentName查看环境配置，env.ApplicationName查看项目名字

* 在开发机器上，我们在**launchsettings.json**文件中设置环境变量

* 而Staging或者Production的变量，我们尽量在**操作系统中设置**

* 使用IHosttingEnvironment服务访问运行时环境

* 除了标准环境（Development,Staging,Production）之外，还支持自定义环境（UAT,QA等）

## MVC
1. 将所需的MVC服务添加到asp.net core中的依赖注入容器中
```
    eg:
    public void ConfigureServices(IServiceCollection services)
        {
            //添加MVC服务
            services.AddMvc();
           
        }
```

2. 添加MVC中间件到我们的请求处理管道中
> .net core2.2使用的是app.UseMvcWithDefaultRoute(),在3.x版本中直接使用会报错，应使用app.UseRouting()+app.UseEndpoints()
```
    eg:
    //添加MVC中间件
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                 );
            });
```
3. 创建Controllers文件夹，右键点击，选择添加选择控制器，选择MVC-空

4. AddMvcCore只包含了 核心的MVC功能。AddMvc包含了依赖于MvcCore 以及相关的第三方常用的服务和方法

# 2021-03-19

## Model

* 在Model文件夹内，创建相关的Model类
    * 添加相关数据属性    
        * '///'添加说明
        * prop 两次tab件快速添加属性

* 在Model文件夹内，创建相关Model类的数据接口

* 在Startup.cs文件中的ConfigureServices服务中，绑定接口与数据源,此方法为**依赖注入**
    * `services.AddSingleton<a, b>();`,b为数据源，a为接口,b服务依赖于a服务
    ```
        eg:
        public void ConfigureServices(IServiceCollection services)
        {
            //添加MVC服务
            services.AddMvc();

            services.AddSingleton<IStudentRepository, MockStudentRepository>();
           
        }
    ```

* 在Controller中，调用Model数据接口,获取数据
    * 使用构造函数注入的方式注入接口
        ```
            eg:
                private readonly IStudentRepository _studentRepository;
                //使用构造函数注入的方式注入IStudentRepository
                public HomeController(IStudentRepository studentsRepository)
                {
                    _studentRepository = studentsRepository;
                }
                //public string Index()
                //{
                //    return "Hello MVC";
                //}

                public string Index() 
                {
                    return _studentRepository.GetStudents(1).Name;
                    //return Json(new { id = "1", name = "张三" });
                }
        ```

* First:返回序列中的第一个元素。 FirstOrDefault:返回序列中的第一个元素；如果序列中不包含任何元素，则返回默认值.
```
    eg:
        private List<Students> _studentList;

        public MockStudentRepository()
        {
            _studentList = new List<Students>()
            { 
                new Students(){ Id = 1, Name = "小明", ClassName = "一年级",Emaill = "2821@qq.com"},
                new Students(){ Id = 2, Name = "小红", ClassName = "一年级",Emaill = "2822@qq.com"},
                new Students(){ Id = 3, Name = "小强", ClassName = "二年级",Emaill = "2823@qq.com"},
                new Students(){ Id = 4, Name = "小李", ClassName = "三年级",Emaill = "2824@qq.com"},
                new Students(){ Id = 5, Name = "小狗", ClassName = "四年级",Emaill = "2825@qq.com"},
            
            };
        }



        public Students GetStudents(int id) 
        {
            return _studentList.FirstOrDefault(a => a.Id == id);
        }

```
## 依赖注入
* ASP.NET Core 依赖注入容器注册服务
    * AddSingleton()
    * AddTransient()
    * AddScoped()

* 依赖注入的亮点
    * 低耦合
    * 提供了高测试性，使单元测试更加容易

# 2021-03-22

## MVC中的Controlle
* 当浏览器的请求到达我们的应用程序时，作为MVC设计模式中的控制器，他处理传入http请求并响应用户操作。

* 控制器构建模型（Model）

* 如果我们正在构建API,则将模型数据返回给调用方

* 或者选择 "View 视图" 并将模型数据传递到视图，然后视图生成所需的HTML来显示数据。

* 需要返回xml格式数据，需要在Startup.cs服务中，使用 ` services.AddSingleton<IStudentRepository, MockStudentRepository>(); `

## MVC中的View
* 视图文件具有.cshtml的扩展名

* 视图文件是带有嵌入Razor标记的HTML模板。

* 可通过右键点击视图方法，选择添加视图，创建视图模型