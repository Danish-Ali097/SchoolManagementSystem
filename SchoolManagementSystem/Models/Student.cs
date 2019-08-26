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

    public partial class Student
    {
        public int Id { get; set; }
        [Display(Name = "Student Name")]
        public string Std_Name { get; set; }
        [Display(Name = "Father Name")]
        public string Father_Name { get; set; }
        [Range(minimum:1,maximum:100,ErrorMessage ="Age must be greater then 0 and less or equal to 100")]
        public Nullable<int> Age { get; set; }
        public string Address { get; set; }
        [Display(Name = "Mobile Number"),RegularExpression(pattern: @"^([0]{1})+([3]{1})+([0-9]{2})+([0-9]{7})$", ErrorMessage ="Contact number must be 03xxxxxxxxx format")]
        public Nullable<int> Contact_Number { get; set; }
        [Required, Display(Name = "Reg No.")]
        public string Reg_No { get; set; }
        public string Email { get; set; }
        [Required, StringLength(12,MinimumLength = 8,ErrorMessage ="password must be atleast 8 characters long and maximum of length 12 characters")]
        public string Password { get; set; }
        public Nullable<int> Class_Id { get; set; }
        public string Image_Path { get; set; }
    }
}
