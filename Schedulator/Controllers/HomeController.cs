using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Schedulator.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public Func<string> GetUserId; //For testing
        public Func<string, bool> IsInRole; //For testing

        private ApplicationDbContext db = new ApplicationDbContext();
        public HomeController()
        {
            GetUserId = () => User.Identity.GetUserId();
            IsInRole = (string role) => User.IsInRole(role);
        }
        public ActionResult Index()
        {
            string temp = GetUserId();
            if (IsInRole("Student"))
                return View(db.Users.Find(GetUserId()));
            else
                return Redirect("ProgramDirector");

        }
    }
}