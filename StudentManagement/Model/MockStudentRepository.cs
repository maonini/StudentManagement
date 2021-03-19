using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class MockStudentRepository : IStudentRepository
    {
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
    }
}
