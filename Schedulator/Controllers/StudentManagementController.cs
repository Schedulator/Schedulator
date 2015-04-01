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
        public ActionResult Index()
        {
            List<ApplicationUser> applicationUser = db.Users.ToList();
            return View(db.Users.ToList().Where(user => user.Roles.First().RoleId == userRole));
        }

        public ActionResult findStudent(string Name)
        {
            ApplicationUser user = db.Users.First(u => (u.FirstName + " " + u.LastName) == Name);
            return View(user);
        }

	}
}