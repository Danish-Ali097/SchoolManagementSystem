using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Models
{
    public class ParentClassSubjectModel
    {
        public Class Class { get; set; }
        public Subject Subject { get; set; }
        public IEnumerable<Class> ClassList { get; set; }
        public IEnumerable<Subject> SubjectList { get; set; }
    }
}