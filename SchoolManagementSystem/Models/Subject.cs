//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using SchoolManagementSystem.Models;

    public partial class Subject
    {
        public int Id { get; set; }
        [Display(Name = "Subject Name")]
        public string Subject_Name { get; set; }
        [Display(Name = "Instructor Name")]
        public string Instructor_Name { get; set; }
        public Nullable<int> Class_Id { get; set; }
        [Display(Name ="Book Name")]
        public string Book_Name { get; set; }

        [NotMapped]
        public List<Class> Classes { set; get; }
    }
}
