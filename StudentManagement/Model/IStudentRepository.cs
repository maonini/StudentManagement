using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    /// <summary>
    /// 获取学生信息
    /// </summary>
    public interface IStudentRepository
    {
        Students GetStudents(int id);
    }
}
