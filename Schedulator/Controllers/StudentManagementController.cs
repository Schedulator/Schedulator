using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Schedulator.Models;

namespace Schedulator.Controllers
{
    public class StudentManagementController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private string userRole = "f61ed43a-fa52-4031-9cbf-753a8e861fd3";

        //
        // GET: /StudentManagement/
        public ActionResult Index(string searchTerm = null)
        {
            List<ApplicationUser> applicationUser = db.Users.Where( u => searchTerm == null || searchTerm.Contains(u.FirstName) || searchTerm.Contains(u.LastName)).ToList();
            return View(applicationUser.Where(user => user.Roles.First().RoleId == userRole));
        }

        public ActionResult Details(string id)
        {
            ApplicationUser user = db.Users.First(u => u.Id == id );            
            return View(user);           
        }

        public ActionResult ModifySchedule(string id)
        {
            ApplicationUser user = db.Users.First(u => u.Id == id);  
            return View(user.Schedules.ToList());
        }

        [HttpGet]
        public ActionResult SeeGradeToEnrollment(string id)
        {
            ApplicationUser user = db.Users.First(u => u.Id == id);
            return View(user.Schedules.SelectMany(u => u.Enrollments).ToList());
        }

        [HttpGet]
        public ActionResult AddGradeToEnrollment(int id)
        {
            Enrollment enrollment = db.Enrollment.Find(id);
            return View(enrollment);
        }

        [HttpPost]
        public ActionResult AddGradeToEnrollment(Enrollment enrollment)
        {            
            var user = db.Enrollment.AsNoTracking().First(u => u.EnrollmentID == enrollment.EnrollmentID).Schedule.ApplicationUser.Id;

            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SeeGradeToEnrollment", new { id = user });
            }
            return View(enrollment);
        } 
	}
}