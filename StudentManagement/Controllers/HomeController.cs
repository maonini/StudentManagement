using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
        //public string Index()
        //{
        //    return "Hello MVC";
        //}

        public JsonResult Index() 
        {
            return Json(new { id = "1", name = "张三" });
        }
    }
}
