using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schedulator.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Schedulator.Controllers;
using System.Web.Script.Serialization;
namespace Schedulator.Tests.Controllers
{
    [TestClass]
    public class ScheduleGeneratorTest
    {
        [TestMethod]
        public void ScheduleGenerateWithStudentManuallyAddedCourses()
        {
            List<Course> course = new List<Course>();

            ApplicationDbContext db = new ApplicationDbContext();
            
            Preference preference = new Preference {  Semester = db.Semesters.Where(n => n.Season == Season.Fall).FirstOrDefault() };
            List<Course> courses = db.Courses.ToList();
            preference.Courses = new List<Course>();
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 232 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 248 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 201 && n.CourseLetters == "ENGR").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 213 && n.CourseLetters == "ENGR").FirstOrDefault());
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };

            Program program = db.Program.FirstOrDefault();
            
            scheduleGenerator.GenerateSchedules(db.Courses.ToList(), db.Enrollment.ToList());
 
        }

        /*------------"CoursesViewJson()" method testing------------------*/
        [TestMethod]
        public void verifyJSONCourseViews()
        {
            ApplicationDbContext db1 = new ApplicationDbContext();
        
            List<Course> db1Courses= db1.Courses.ToList();
            List<Schedulator.Models.ScheduleGenerator.CourseView> courseViews = new List<Schedulator.Models.ScheduleGenerator.CourseView>();
            foreach (Course course in db1Courses) {
                courseViews.Add(new Schedulator.Models.ScheduleGenerator.CourseView { CourseId = course.CourseID, label = course.CourseLetters + " " + course.CourseNumber });
            }

            string expected = new JavaScriptSerializer().Serialize(courseViews);

            ScheduleGeneratorController testSchedGenCon = new ScheduleGeneratorController();

            JsonResult actual = testSchedGenCon.CoursesViewJson();
            string actualJson = new JavaScriptSerializer().Serialize(actual.Data);
            Assert.AreEqual(actualJson, expected);

            

        }

         /*------------"StudentCourseSequence()" method testing------------------*/
        [TestMethod]
        public void test3()
        {
        }


        /*------------"GenerateScheduleAndRegisterSchedule()" method testing------------------*/
        [TestMethod]
        public void verifyGenerateAndRegisterScheduleWithController()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = db.Users.Where(n => n.Email == "harleymc@gmail.com").FirstOrDefault().Id;
            ScheduleGeneratorController testScheGenCon = new ScheduleGeneratorController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };
            List<string> courseCodes = new List<string> {"COMP 232", "COMP 248", "ENGR 201", "ENGR 213" };
            ViewResult view = testScheGenCon.GenerateSchedules(courseCodes,"Fall", null) as ViewResult;
            ScheduleGenerator schedGenerator =  testScheGenCon.scheduleGenerator;

            testScheGenCon.sectionIds = new List<int>();

            Schedule scheduleToRegister = schedGenerator.Schedules.FirstOrDefault().FirstOrDefault();
            foreach (Enrollment enrollmentsInSchedule in scheduleToRegister.Enrollments)
                testScheGenCon.sectionIds.Add(enrollmentsInSchedule.Section.SectionId);

            testScheGenCon.RegisterSchedule();
            ApplicationUser user = db.Users.Where(n => n.Email == "harleymc@gmail.com").FirstOrDefault();
            Schedule registeredSchedule = db.Users.Where(n => n.Email == "harleymc@gmail.com").FirstOrDefault().Schedules.Where(n => n.Semester.SemesterID == scheduleToRegister.Semester.SemesterID).FirstOrDefault();

            // Check if the schedule from the db is the same that was generated and saved
            for (int i = 0; i < registeredSchedule.Enrollments.Count(); i++ )
                Assert.IsTrue(registeredSchedule.Enrollments.ToList()[i].Section.SectionId == scheduleToRegister.Enrollments.ToList()[i].Section.SectionId);
        }
        /*-------------







    }
}
