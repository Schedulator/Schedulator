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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSchedule(string semester)
        {   
            List<Schedule> schedules = new List<Schedule>();
            string user = db.Users.Find(User.Identity.GetUserId()).Email;
            User.Identity.GetUserId();
            bool registeredSchedule = true;
            if (semester.Contains("Saved"))
                registeredSchedule = false;
            if (semester.Contains("Fall"))
                schedules = db.Schedule.Where(t => t.Semester.Season == Season.Fall && t.ApplicationUser.Email == user && t.IsRegisteredSchedule == registeredSchedule).ToList();
            else if (semester.Contains("Winter"))
                schedules = db.Schedule.Where(t => t.Semester.Season == Season.Winter && t.ApplicationUser.Email == user && t.IsRegisteredSchedule == registeredSchedule).ToList();
            else if (semester.Contains("Summer"))
                schedules = db.Schedule.Where(t => (t.Semester.Season == Season.Summer1 || t.Semester.Season == Season.Summer2) && t.ApplicationUser.Email == user && t.IsRegisteredSchedule == registeredSchedule).ToList();
            return PartialView("StudentScheduleResultPartial", schedules);
        }
        public ActionResult ManageSchedule(List<int> sectionIds, List<int> scheduleIds)
        {
            List<Schedule> scheduleList = new List<Schedule>();
            foreach (int scheduleId in scheduleIds)
            {
                Schedule schedule = db.Schedule.Where(n => n.ScheduleId == scheduleId).FirstOrDefault();
                foreach (int sectionId in sectionIds)
                {
                    Enrollment enrollment = schedule.Enrollments.Where(n => n.Section.SectionId == sectionId).FirstOrDefault();
                    if (enrollment != null)
                    {
                        schedule.Enrollments.Remove(enrollment);
                        db.Enrollment.Remove(enrollment);

                    }
                    db.Entry(schedule).State = System.Data.Entity.EntityState.Modified;
                    scheduleList.Add(schedule);
                }
            }
            db.SaveChanges();
            return PartialView("_ScheduleAndLegend", scheduleList);
        }

    }
}