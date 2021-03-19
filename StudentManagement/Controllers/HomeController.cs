using Microsoft.AspNetCore.Mvc;
using StudentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
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
    }
}
