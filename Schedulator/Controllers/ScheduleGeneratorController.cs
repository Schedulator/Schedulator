﻿using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Schedulator.Controllers
{
    [Authorize]
    public class ScheduleGeneratorController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
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
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            List<Enrollment> studentEnrollments = new List<Enrollment>();
            foreach (Schedule schedule in user.Schedules)
            {
                studentEnrollments.AddRange(schedule.Enrollments);
            }
            return PartialView("_RecommendedCourseList", user.Program.RecommendedCourseForStudent(studentEnrollments));
        }
        [HttpPost]
        public ActionResult RegisterSchedule()
        {
            List<string> keys = Request.Form.AllKeys.Where(n => n.Contains("radioButtonSectionGroup")).ToList();

            List<Schedule> schedules = new List<Schedule>();
            bool isRegisteredSchedule = false;
            if (Request.Form["register"] != null )
                isRegisteredSchedule = true;


            foreach( string key in keys)
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
                        schedules.Add(new Schedule { ApplicationUser = db.Users.Find(User.Identity.GetUserId()), Semester = section.Lecture.Semester, IsRegisteredSchedule = isRegisteredSchedule, Enrollments = new List<Enrollment>() { new Enrollment { Course = section.Lecture.Course, Section = section } } });

                }
            }
            foreach (Schedule schedule in schedules)
            {
                if (isRegisteredSchedule)
                    schedule.RegisterSchedule();
                else
                    schedule.SaveSchedule();

                db.Schedule.Add(schedule);
                db.SaveChanges();
            }

            return PartialView("RegisterSuccessPartial");
        }
        [HttpPost]
        public ActionResult GenerateSchedules(List<String> courseCode, String semester, List<String> timeOption) {

            ScheduleGenerator scheduler = new ScheduleGenerator { Preference = new Preference() };
            Season sem;
            switch (semester)
            {
                case "Summer": sem = Season.Summer1;
                    break;
                case "Fall": sem = Season.Fall;
                    break;
                case "Winter": sem = Season.Winter;
                    break;
                default: sem = Season.Fall;
                    break;
            }

            double startTime = 0;
            double endTime = 0;
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
            endTime = (endTime == 0) ? 1440 : endTime;

            // Some test data for now to display generated schedules
            //Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == Season.Summer1 || n.Season == Season.Summer2).FirstOrDefault(), StartTime = 0, EndTime = 1440 };
            Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == sem).FirstOrDefault(), StartTime = startTime, EndTime = endTime };
            List<Course> courses = db.Courses.ToList();
            preference.Courses = new List<Course>();
            
            foreach(var code in courseCode) {
                String[] courseID = code.Split(' ');
                var courseLetter = courseID[0];
                int courseNumber = Int32.Parse(courseID[1]);

                preference.Courses.Add(courses.Where(n => n.CourseNumber == courseNumber && n.CourseLetters == courseLetter).FirstOrDefault());
            }
            
            //preference.Courses.Add(courses.Where(n => n.CourseNumber == cNumber[1] && n.CourseLetters == cLetter[0]).FirstOrDefault());

            
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };

            Program program = db.Program.FirstOrDefault();

            scheduleGenerator.GenerateSchedules(db.Courses.ToList(), db.Enrollment.ToList(), program);


            return PartialView("GenScheduleResultPartial", scheduleGenerator);
        }

    }
}
