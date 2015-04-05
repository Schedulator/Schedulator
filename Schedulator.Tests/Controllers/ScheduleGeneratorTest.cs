using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schedulator.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Schedulator.Controllers;

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

          //  JsonResult expected = Json(courseViews, JsonRequestBehavior.AllowGet);

            ScheduleGeneratorController testSchedGenCon = new ScheduleGeneratorController();

            JsonResult actual = testSchedGenCon.CoursesViewJson();

         //   Assert.AreEqual(actual, expected);


        }

         /*------------"StudentCourseSequence()" method testing------------------*/
        [TestMethod]
        public void test3()
        {
        }


        /*------------"RegisterSchedule()" method testing------------------*/
        [TestMethod]
        public void test4()
        {
        }







    }
}
