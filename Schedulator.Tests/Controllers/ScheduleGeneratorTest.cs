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
        public void verifyStudentJSONCourseViews()
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the student test profiles
            string userId = db.Users.Where(n => n.Email == "student1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create list of all current courses
            List<Course> db1Courses = db.Courses.ToList();
            List<Schedulator.Models.ScheduleGenerator.CourseView> courseViews = new List<Schedulator.Models.ScheduleGenerator.CourseView>();
            foreach (Course course in db1Courses) {
                courseViews.Add(new Schedulator.Models.ScheduleGenerator.CourseView { CourseId = course.CourseID, label = course.CourseLetters + " " + course.CourseNumber });
            }

            //Store expected value
            //JsonResult studentExpected = Json(courseViews, JsonRequestBehavior.AllowGet);
            JsonResult studentExpected = null;


            //Create new controller object, call "CoursesViewJson()" method
            ScheduleGeneratorController testSchedGenCon = new ScheduleGeneratorController();
            JsonResult actual = testSchedGenCon.CoursesViewJson();

            //Check if method returns the expected output for a program director
            Assert.AreEqual(actual, studentExpected);


        }


        [TestMethod]
        public void verifyProgramDirectorJSONCourseViews()
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the program director test profiles
            string userId = db.Users.Where(n => n.Email == "programdirector1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create list of all current courses
            List<Course> db1Courses = db.Courses.ToList();
            List<Schedulator.Models.ScheduleGenerator.CourseView> courseViews = new List<Schedulator.Models.ScheduleGenerator.CourseView>();
            foreach (Course course in db1Courses)
            {
                courseViews.Add(new Schedulator.Models.ScheduleGenerator.CourseView { CourseId = course.CourseID, label = course.CourseLetters + " " + course.CourseNumber });
            }

            //Store expected value
            //JsonResult programDirectorExpected = Json(courseViews, JsonRequestBehavior.AllowGet);
            JsonResult programDirectorExpected = null;


            //Create new controller object, call "CoursesViewJson()" method 
            ScheduleGeneratorController testSchedGenCon = new ScheduleGeneratorController();
            JsonResult actual = testSchedGenCon.CoursesViewJson();

            //Check if method returns the expected output for a program director
            Assert.AreEqual(actual, programDirectorExpected);


        }

        [TestMethod]
        public void verifyStudentDirectorSameJSONCourseViews()
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the program director test profiles
            string userId = db.Users.Where(n => n.Email == "programdirector1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create new controller object, call "CoursesViewJson()" method 
            ScheduleGeneratorController testSchedGenCon1 = new ScheduleGeneratorController();
            JsonResult programDirectorResult = testSchedGenCon1.CoursesViewJson();

            //Set the user to be one of the student test profiles
            string userId2 = db.Users.Where(n => n.Email == "student1@test.ca").FirstOrDefault().Id;

            //Log into student account 
            controller.GetUserId = () => userId2;

             //Create new controller object, call "CoursesViewJson()" method 
            ScheduleGeneratorController testSchedGenCon2 = new ScheduleGeneratorController();
            JsonResult studentResult = testSchedGenCon2.CoursesViewJson();

            //Check if method returns the expected output for a program director
            Assert.AreEqual(programDirectorResult, studentResult);
        }






         /*------------"StudentCourseSequence()" method testing------------------*/
       
        
        //TODO: test output of function with valid student credentials
        [TestMethod]
        public void studentProgramSequenceVerifyStudentCourseSequence()
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the student test profiles
            string userId = db.Users.Where(n => n.Email == "student1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create new controller object, call "studentCourseSequence()" method 
            ScheduleGeneratorController testSchedGenCon1 = new ScheduleGeneratorController();
            ActionResult studentResult = testSchedGenCon1.StudentsCourseSequence();

        }


        //Test output when program director is logged in (should throw exception)
        [TestMethod]
        public void programDirectoreVerifyStudentCourseSequence()
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the student test profiles
            string userId = db.Users.Where(n => n.Email == "programDirector1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create new controller object, call "studentCourseSequence()" method 
            ScheduleGeneratorController testSchedGenCon1 = new ScheduleGeneratorController();
            ActionResult programDirectorResult = testSchedGenCon1.StudentsCourseSequence();

            //Expected exception
            ArgumentNullException programDirectorExpected = new ArgumentNullException("User missing program.");

            //Check if method throws appropriate error when program director is logged in
            Assert.AreEqual(programDirectorResult, programDirectorExpected);
        }


        //Test output when student without schedule is logged in (should throw exception)
        [TestMethod]
        public void StudentVerifyStudentCourseSequence()
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the student test profiles
            string userId = db.Users.Where(n => n.Email == "student1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create new controller object, call "studentCourseSequence()" method 
            ScheduleGeneratorController testSchedGenCon1 = new ScheduleGeneratorController();
            ActionResult studentResult = testSchedGenCon1.StudentsCourseSequence();

            //Expected exception
            ArgumentNullException studentExpected = new ArgumentNullException("User missing schedule(s)");

            //Check if method throws appropriate error when program director is logged in
            Assert.AreEqual(studentResult, studentExpected);
        }


        /*------------"RegisterSchedule()" method testing------------------*/
        [TestMethod]
        public void test4()
        {


        }


        /*------------"GenerateSchedules()" method testing------------------*/
        [TestMethod]
        public void studentNullCourseGenerateSchedules () 
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the student test profiles
            string userId = db.Users.Where(n => n.Email == "student1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create new controller object, call "GenerateSchedules()" method 
            ScheduleGeneratorController testSchedGenCon1 = new ScheduleGeneratorController();
            ActionResult studentResult = testSchedGenCon1.GenerateSchedules(null, "Fall", null);

            //Create JSON object of alert message
            //ActionResult studentExpected = Json(new { Success = false, Message = "Please add one or more courses." });
            JsonResult studentExpected = null;
            //Check if method generates appropriate alert message
            Assert.AreEqual(studentResult, studentExpected);
        }



        //Test if a null semester generates the same schedule as if "Fall" was specified
        [TestMethod]
        public void studentNullSemesterGenerateSchedules()
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the student test profiles
            string userId = db.Users.Where(n => n.Email == "student1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create a list of course codes
            List<string> courseCodes = new List<string>{ "COMP 248", "ENGR 201", "COMP 232" };

            //Create new controller object, call "GenerateSchedules()" method 
            ScheduleGeneratorController testSchedGenCon1 = new ScheduleGeneratorController();
            ActionResult studentResult = testSchedGenCon1.GenerateSchedules(courseCodes, null, null);

            //Call "GenerateSchedules()" method specifying "Fall" as the semester
            ActionResult studentExpected = testSchedGenCon1.GenerateSchedules(courseCodes, "Fall", null);

            //Check if method generates appropriate schedule
            Assert.AreEqual(studentResult, studentExpected);
        }

        //Check if function returns error message when no time is specified in function call
        [TestMethod]
        public void StudentNullTimeGenerateSchedules()
        {
            //Create new database instance
            ApplicationDbContext db = new ApplicationDbContext();

            //Set the user to be one of the student test profiles
            string userId = db.Users.Where(n => n.Email == "student1@test.ca").FirstOrDefault().Id;
            HomeController controller = new HomeController()
            {
                GetUserId = () => userId,
                IsInRole = (role) => true
            };

            //Create a list of course codes
            List<string> courseCodes = new List<string> { "COMP 248", "ENGR 201", "COMP 232" };

            //Create new controller object, call "GenerateSchedules()" method 
            ScheduleGeneratorController testSchedGenCon1 = new ScheduleGeneratorController();
            ActionResult studentResult = testSchedGenCon1.GenerateSchedules(courseCodes, "Fall", null);

            //Call "GenerateSchedules()" method specifying "Fall" as the semester
            //ActionResult studentExpected = Json(new { Success = false, Message = "Please add a time option." });
            JsonResult studentExpected = null;

            //Check if method generates appropriate schedule
            Assert.AreEqual(studentResult, studentExpected);
        }







    }
}
