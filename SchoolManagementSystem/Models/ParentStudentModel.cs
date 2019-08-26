using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Models
{
    public class ParentStudentModel
    {
        public Student Student { set; get; }
        public Class Class { set; get; }
        public List<Class> ClassList { set; get; }
    }
}