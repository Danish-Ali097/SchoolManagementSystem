using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class SMSController : Controller
    {
        schoolDBEntities db = new schoolDBEntities();
        // GET: SMS
        public ActionResult Index()
        {
            return View();
        }

        #region Admin Login and Signup

        // Admin Login
        public ActionResult AdminLogin()
        {
            return View();
        }

        //GET: Admin Login
        [HttpPost]
        public ActionResult AdminLogin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var res = db.Admins.ToList();
                var count = 0;
                foreach (var x in res)
                {
                    if (x.Email == admin.Email && x.Password == admin.Password)
                    {
                        count++;
                        admin = x;
                    }
                }
                if (count > 0)
                {
                    Session.Add("AdminLgnFlag", admin);
                    return RedirectToAction("AdminDashboard");
                }
            }
            else
            {
                return View();
            }
            ViewData["AlgnFailed"] = "Email or Password may Incorrect!";
            return View();
        }

        //Admin Logout
        public ActionResult AdminLogout()
        {
            Session.Remove("AdminLgnFlag");
            return RedirectToAction("Index");
        }

        // Admin Signup
        public ActionResult AdminSignup()
        {
            return View();
        }


        //POST: Admin Signup
        [HttpPost]
        public ActionResult AdminSignup(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var res = db.Admins.ToList();
                var count = 0;
                foreach(var x in res)
                {
                    if(x.Email == admin.Email)
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    ViewData["A_exist"] = "Email Already Exists!";
                    return View();
                }
                db.Admins.Add(admin);
                db.SaveChanges();
            }
            ViewData["A_added"] = "Admin added!";
            return View();
        }

        #endregion

        #region Student Login

        // Student Login
        public ActionResult StudentLogin()
        {
            return View();
        }

        // Get: Student Login
        [HttpPost]
        public ActionResult StudentLogin(Student student)
        {
            if (ModelState.IsValid)
            {
                var res = db.Students.ToList();
                var count = 0;
                foreach (var x in res)
                {
                    if (x.Reg_No == student.Reg_No && x.Password == student.Password)
                    {
                        count++;
                        student = x;
                    }
                }
                if (count > 0)
                {
                    Session.Add("StudentLgnFlag", student);
                    return RedirectToAction("StudentDashboard");
                }
            }
            else
            {
                return View();
            }
            ViewData["SlgnFailed"] = "Email or Password may Incorrect!";
            return View();
        }

        #endregion

        #region Admin Dashboard
        //Admin Dashboard
        public ActionResult AdminDashboard()
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                ViewData["classCount"] = (int)db.Classes.Count<Class>();
                ViewData["studentCount"] = (int)db.Students.Count<Student>();
                return View( db.Students.ToList<Student>());
            }
            return RedirectToAction("Index");
        }

        #region Student C.R.U.D

        //Add Student

        public ActionResult AddStudent()
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                ParentStudentModel model = new ParentStudentModel();
                model.ClassList = db.Classes.ToList<Class>();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddStudent(ParentStudentModel parentStudentModel)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                

                Student student = new Student();
                student.Std_Name = parentStudentModel.Student.Std_Name;
                student.Father_Name = parentStudentModel.Student.Father_Name;
                student.Age = parentStudentModel.Student.Age;
                student.Address = parentStudentModel.Student.Address;
                student.Contact_Number = parentStudentModel.Student.Contact_Number;
                student.Email = parentStudentModel.Student.Email;
                student.Class_Id = parentStudentModel.Student.Class_Id;

                int temp = db.Students.Count<Student>();

                student.Reg_No = GetRegno(temp);
                student.Password = RandomString();

                db.Students.Add(student);
                db.SaveChanges();

                //smtp
                #region smtp
                var senderEmail = new MailAddress("danishali20021996@gmail.com", "Danish");
                var receiverEmail = new MailAddress(student.Email, "Receiver");
                var password = "da163998";
                var sub = "Portal Username and Password";
                var body = "Regno: "+student.Reg_No+"\nPassword: "+student.Password;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
                #endregion
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                ViewData["StudentAdded"] = "Successfully added!";
                ParentStudentModel model = new ParentStudentModel();
                model.ClassList = db.Classes.ToList<Class>();
                return View(model);

                //smtp end
            }
            return RedirectToAction("Index");



        }
        #endregion

        #region Subject C.R.U.D.
        //Add Subject
        public ActionResult AddSubject()
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                return View();
            }
            return RedirectToAction("Index");
        }

        //POST: AddSubject
        [HttpPost]
        public ActionResult AddSubject(Subject subject)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                if (ModelState.IsValid)
                {
                    db.Subjects.Add(subject);
                    db.SaveChanges();
                    ViewData["SbjAddedSuccess"] = "Subject Added!";
                    return View();
                }
                ViewData["SbjAddedFailed"] = "Subject not Added!";
                return View(); 
            }
            return RedirectToAction("Index");
        }

        //ViewSubject
        public ActionResult ViewSubject()
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                return View(db.Subjects.ToList<Subject>());
            }
            return RedirectToAction("Index");
        }

        //EditSubject
        public ActionResult EditSubject(int id)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                Subject subject = new Subject();
                subject = db.Subjects.Find(id);
                subject.Classes = db.Classes.ToList<Class>();
                return View(subject);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditSubject(Subject subject)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                db.Entry(subject).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewSubject");
            }
            return RedirectToAction("Index");
        }

        //DeleteSubject
        public ActionResult DeleteSubject(int id)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                ParentClassSubjectModel model = new ParentClassSubjectModel();
                model.Subject = db.Subjects.Find(id);
                var temp = db.Classes.ToList<Class>();
                foreach (var x in temp)
                {
                    if (x.Id == model.Subject.Class_Id )
                    {
                        model.Class = x;
                    }
                }
               
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteSubject(ParentClassSubjectModel model)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                var temp = db.Subjects.Find(model.Subject.Id);
                if (temp != null)
                {
                    db.Subjects.Remove(temp);
                    db.SaveChanges();
                    return RedirectToAction("ViewSubject");
                }
                return View();
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Class C.R.U.D.
        //Add Class
        public ActionResult AddClass()
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                return View(); 
            }
            return RedirectToAction("Index");
        }
        //POST: AddClass
        [HttpPost]
        public ActionResult AddClass(Class Class)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var tmp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = tmp;
                if (ModelState.IsValid)
                {
                    db.Classes.Add(Class);
                    db.SaveChanges();
                    ViewData["ClassAdded"]="Clas Added";
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        //ViewClass
        public ActionResult GetClass()
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var temp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = temp;
                
                return View(db.Classes.ToList<Class>());//return list of class
            }
            return RedirectToAction("Index");
        }

        //EditClass
        public ActionResult EditClass( int id)
        {
            if (Session["AdminLgnFlag"] != null)
            {
                var temp = (Admin)Session["AdminLgnFlag"];
                ViewData["admin"] = temp;

                return View();
            }
            return RedirectToAction("Index");
        }
        //DeleteClass

        #endregion//class

        #endregion//admin
        //generate randon regno
        public static string GetRegno(int count)
        {
            string str = System.DateTime.Now.Year.ToString();
            str += count+1;

            return str;
        }
        // Generate a random string with a given size  
        public string RandomString()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < 8; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}