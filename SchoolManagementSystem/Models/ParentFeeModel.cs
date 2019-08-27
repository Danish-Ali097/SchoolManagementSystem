using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class ParentFeeModel
    {
        public Student Student { set; get; }
        public Class Class { set; get; }
        public Fee Fee { set; get; }
    }
}