using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Data.Entity.Validation;

namespace Schedulator.Controllers
{
    [Authorize]
    public class ScheduleGeneratorController : Controller
    {
        public Func<string> GetUserId; //For testing
        public Func<string, bool> IsInRole; //For testing
        public List<int> sectionIds;
        public ScheduleGenerator scheduleGenerator;

        private ApplicationDbContext db = new ApplicationDbContext();
        public ScheduleGeneratorController()
        {
            GetUserId = () => User.Identity.GetUserId();
            IsInRole = (string role) => User.IsInRole(role);
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CoursesViewJson()
        {
            List<Course> courses = db.Courses.ToList();
            List<Schedulator.Models.ScheduleGenerator.CourseView> coursesView = new List<Schedulator.Models.ScheduleGenerator.CourseView>();
            foreach (Course course in courses)
            {
                coursesView.Add(new Schedulator.Models.ScheduleGenerator.CourseView { CourseId = course.CourseID, label = course.CourseLetters + " " + course.CourseNumber });
                
            }
            return Json(coursesView, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentsCourseSequence()
        {
            ApplicationUser user = db.Users.Find(GetUserId());
            
            if(user.Program == null){
                throw new System.ArgumentNullException("User missing program.");
            }else{
                List<Enrollment> studentEnrollments = new List<Enrollment>();
                foreach (Schedule schedule in user.Schedules)
                    studentEnrollments.AddRange(schedule.Enrollments);
                return PartialView("_RecommendedCourseList", user.Program.RecommendedCourseForStudent(studentEnrollments,
                                                             user.Schedules.Where(n =>n.Semester.Season == Season.Fall && n.Semester.SemesterStart.Year == 2014).FirstOrDefault(),
                                                             user.Schedules.Where(n => n.Semester.Season == Season.Winter && n.Semester.SemesterStart.Year == 2015).FirstOrDefault(), 
                                                             user.Schedules.Where(n =>(n.Semester.Season == Season.Summer1 || n.Semester.Season == Season.Summer2) && n.Semester.SemesterStart.Year == 2015).ToList()));
            }
        }
        [HttpPost]
        public ActionResult RegisterSchedule()
        {
            List<string> keys = new List<string>();
            if (sectionIds == null)
                 keys = Request.Form.AllKeys.Where(n => n.Contains("radioButtonSectionGroup")).ToList();
            List<Schedule> schedules = new List<Schedule>();
            if (sectionIds == null)
            {
                foreach (string key in keys)
                {
                    int sectionId = Convert.ToInt32(Request.Form[key]);
                    Section section = db.Section.Where(n => n.SectionId == sectionId).FirstOrDefault();
                    if (section != null)
                    {
                        bool noScheduleForSemester = true;
                        foreach (Schedule schedule in schedules)
                        {
                            if (schedule.Semester == section.Lecture.Semester)
                            {
                                schedule.Enrollments.Add(new Enrollment { Course = section.Lecture.Course, Section = section });
                                noScheduleForSemester = false;
                                break;
                            }
                        }
                        if (noScheduleForSemester)
                            schedules.Add(new Schedule { ApplicationUser = db.Users.Find(GetUserId()), Semester = section.Lecture.Semester, IsRegisteredSchedule = true, Enrollments = new List<Enrollment>() { new Enrollment { Course = section.Lecture.Course, Section = section } } });

                    }
                }
            }
            else
            {
                foreach (int sectionId in sectionIds)
                {
                    Section section = db.Section.Where(n => n.SectionId == sectionId).FirstOrDefault();
                    if (section != null)
                    {
                        bool noScheduleForSemester = true;
                        foreach (Schedule schedule in schedules)
                        {
                            if (schedule.Semester == section.Lecture.Semester)
                            {
                                schedule.Enrollments.Add(new Enrollment { Course = section.Lecture.Course, Section = section });
                                noScheduleForSemester = false;
                                break;
                            }
                        }
                        if (noScheduleForSemester)
                            schedules.Add(new Schedule { ApplicationUser = db.Users.Find(GetUserId()), Semester = section.Lecture.Semester, IsRegisteredSchedule = true, Enrollments = new List<Enrollment>() { new Enrollment { Course = section.Lecture.Course, Section = section } } });

                    }
                }
            }
            foreach (Schedule schedule in schedules)
            {
                schedule.RegisterSchedule();
                db.Schedule.Add(schedule);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return PartialView("RegisterSuccessPartial");
        }
        [HttpPost]
        public ActionResult GenerateSchedules(List<String> courseCode, String semester, List<String> timeOption) {
            if(courseCode == null){
                return Json(new { Success = false, Message = "Please add one or more course." });
            }

            Season season;
            switch (semester)
            {
                case "Summer": season = Season.Summer1;
                    break;
                case "Fall": season = Season.Fall;
                    break;
                case "Winter": season = Season.Winter;
                    break;
                default: season = Season.Fall;
                    break;
            }
            double startTime = 0;
            double endTime = 0;
            if (timeOption != null)
            {
                foreach (string timeType in timeOption)
                {
                    if (timeType == "Morning")
                    {
                        startTime = 1;
                        endTime = 720;
                    }
                    else if (timeType == "Day")
                    {
                        startTime = (startTime == 1) ? startTime : 720;
                        endTime = (endTime < 1080) ? 1080 : endTime;
                    }
                    else if (timeType == "Night")
                    {
                        startTime = (startTime == 1 || startTime == 720) ? startTime : 1080;
                        endTime = (endTime < 1440) ? 1440 : endTime;
                    }
                }
            }
            endTime = (endTime == 0) ? 1440 : endTime;
            Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == season).FirstOrDefault(), StartTime = startTime, EndTime = endTime };
            preference.Courses = new List<Course>();
            
            foreach(var code in courseCode) {
                String[] courseID = code.Split(' ');
                var courseLetter = courseID[0];
                int courseNumber = Int32.Parse(courseID[1]);
                preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == courseNumber && n.CourseLetters == courseLetter).FirstOrDefault());
            }            
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };
            string user = db.Users.Find(GetUserId()).Email;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            scheduleGenerator.GenerateSchedules( db.Enrollment.Where(n => n.Schedule.ApplicationUser.Email == user).ToList());
            scheduleGenerator.Schedules.OrderByDescending(n => n.FirstOrDefault().Days.Count());
            stopWatch.Stop();
            long duration = stopWatch.ElapsedMilliseconds;
            if (scheduleGenerator.Schedules.Count() > 20)
                    scheduleGenerator.Schedules = scheduleGenerator.Schedules.GetRange(0, 20);

            this.scheduleGenerator = scheduleGenerator;
            return PartialView("PagingAndScheduleResultPartial", scheduleGenerator);
        }
        [HttpPost]
        public ActionResult GenerateSchedulesPaging(List<String> courseCode, String semester, List<String> timeOption, int pageNumber)
        {

            Season season;
            switch (semester)
            {
                case "Summer": season = Season.Summer1;
                    break;
                case "Fall": season = Season.Fall;
                    break;
                case "Winter": season = Season.Winter;
                    break;
                default: season = Season.Fall;
                    break;
            }
            double startTime = 0;
            double endTime = 0;
            if (timeOption != null)
            {
                foreach (string timeType in timeOption)
                {
                    if (timeType == "Morning")
                    {
                        startTime = 1;
                        endTime = 720;
                    }
                    else if (timeType == "Day")
                    {
                        startTime = (startTime == 1) ? startTime : 720;
                        endTime = (endTime < 1080) ? 1080 : endTime;
                    }
                    else if (timeType == "Night")
                    {
                        startTime = (startTime == 1 || startTime == 720) ? startTime : 1080;
                        endTime = (endTime < 1440) ? 1440 : endTime;
                    }
                }
            }
            endTime = (endTime == 0) ? 1440 : endTime;
            Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == season).FirstOrDefault(), StartTime = startTime, EndTime = endTime };
            preference.Courses = new List<Course>();

            foreach (var code in courseCode)
            {
                String[] courseID = code.Split(' ');
                var courseLetter = courseID[0];
                int courseNumber = Int32.Parse(courseID[1]);
                preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == courseNumber && n.CourseLetters == courseLetter).FirstOrDefault());
            }
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };
            string user = db.Users.Find(GetUserId()).Email;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            scheduleGenerator.GenerateSchedules(db.Enrollment.Where(n => n.Schedule.ApplicationUser.Email == user).ToList());
            stopWatch.Stop();
            long duration = stopWatch.ElapsedMilliseconds;
            try
            {
                if (scheduleGenerator.Schedules.Count() > 20)
                    scheduleGenerator.Schedules = scheduleGenerator.Schedules.GetRange(pageNumber, 20);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            scheduleGenerator.CurrentPageNumber = pageNumber;
            return PartialView("_GenScheduleResultPartial", scheduleGenerator);
        }

    }
}
