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
            ApplicationDbContext db = new ApplicationDbContext();
            
            ScheduleGenerator scheduler = new ScheduleGenerator { Preference = new Preference()};
            // Some test data for now to display generated schedules
            Preference preference = new Preference { UseCourseSequence = false, Semester = db.Semesters.Where(n => n.Season == Season.Fall).FirstOrDefault() };
            List<Course> courses = db.Courses.ToList();
            preference.Courses = new List<Course>();
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 232 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 248 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 201 && n.CourseLetters == "ENGR").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 213 && n.CourseLetters == "ENGR").FirstOrDefault());
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };

            Program program = db.Program.FirstOrDefault();

            scheduleGenerator.GenerateSchedules(db.Courses.ToList(), db.Enrollment.ToList(), program);

            return View(scheduleGenerator);
        }

    }
}
