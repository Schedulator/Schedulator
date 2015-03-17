using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schedulator.Controllers
{
    public class ScheduleGeneratorController : Controller
    {
        
        public ActionResult Index()
        {
            ScheduleGenerator scheduler = new ScheduleGenerator { Preference = new Preference()};
            ICollection<Course> courses = new List<Course>();
            courses.Add(new Course());
            return View(scheduler);
        }

    }
}
