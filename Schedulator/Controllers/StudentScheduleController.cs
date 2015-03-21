using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schedulator.Controllers
{
    [Authorize]
    public class StudentScheduleController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<Schedule> schedules = db.Schedule.Where(t => t.ApplicationUser.Email == "harleymc@gmail.com").ToList();

            return View(schedules);

        }
    }
}