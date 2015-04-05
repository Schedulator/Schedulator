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
    public class StudentScheduleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string semester)
        {
            return View("Index",null,semester);
        }
        public ActionResult GetSchedule(string semester)
        {   
            List<Schedule> schedules = new List<Schedule>();
            string user = db.Users.Find(User.Identity.GetUserId()).Email;
            User.Identity.GetUserId();

            if (semester.Contains("Fall"))
                schedules = db.Schedule.Where(t => t.Semester.Season == Season.Fall && t.ApplicationUser.Email == user && t.IsRegisteredSchedule == true).ToList();
            else if (semester.Contains("Winter"))
                schedules = db.Schedule.Where(t => t.Semester.Season == Season.Winter && t.ApplicationUser.Email == user && t.IsRegisteredSchedule == true).ToList();
            else if (semester.Contains("Summer"))
                schedules = db.Schedule.Where(t => (t.Semester.Season == Season.Summer1 || t.Semester.Season == Season.Summer2) && t.ApplicationUser.Email == user && t.IsRegisteredSchedule == true).ToList();

            if (schedules.Count() > 0)
                return PartialView("StudentScheduleResultPartial", schedules);
            else
                return PartialView("StudentScheduleNoResultPartial", semester);
        }
        public ActionResult ManageSchedule(List<int> sectionIds, List<int> scheduleIds, string submitType)
        {
            List<Schedule> scheduleList = new List<Schedule>();
            if (submitType == "remove")
            {
                foreach (int scheduleId in scheduleIds)
                {
                    Schedule schedule = db.Schedule.Where(n => n.ScheduleId == scheduleId).FirstOrDefault();
                    schedule.RemoveCourseFromSchedule(sectionIds, db);
                    scheduleList.Add(schedule);
                }
                db.SaveChanges();
            }
            return PartialView("_ScheduleAndLegend", scheduleList);
        }
        public ActionResult GenerateSchedules(List<int> scheduleIds, List<string> courseCode, string semester)
        {
            Season season;
            switch (semester)
            {
                case "Summer1": season = Season.Summer1;
                    break;
                case "Fall": season = Season.Fall;
                    break;
                case "Winter": season = Season.Winter;
                    break;
                default: season = Season.Fall;
                    break;
            }

            Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == season).FirstOrDefault(), StartTime = 0, EndTime = 1440 };
            preference.Courses = new List<Course>();
            
            foreach(var code in courseCode) {
                String[] courseID = code.Split(' ');
                var courseLetter = courseID[0];
                int courseNumber = Int32.Parse(courseID[1]);
                preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == courseNumber && n.CourseLetters == courseLetter).FirstOrDefault());
            }            

            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };
            string user = db.Users.Find(User.Identity.GetUserId()).Email;
            List<List<Section>> sections = new List<List<Section>>();
            foreach( int scheduleId in scheduleIds)
                db.Schedule.Where(n => n.ScheduleId == scheduleId).FirstOrDefault().Enrollments.ToList().ForEach(n => sections.Add(new List<Section>(){ n.Section}));
            scheduleGenerator.GenerateSchedulesUsingSectionsAndCourse(db.Courses.ToList(), sections, db.Enrollment.Where(n => n.Schedule.ApplicationUser.Email == user).ToList());

            return PartialView("_GenScheduleResultPartial", scheduleGenerator);
        }

    }
}