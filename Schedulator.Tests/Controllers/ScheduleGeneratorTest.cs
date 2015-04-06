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

            string studentExpected = new JavaScriptSerializer().Serialize(courseViews);

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
            string programDirectorExpected = new JavaScriptSerializer().Serialize(courseViews);


            //Create new controller object, call "CoursesViewJson()" method 
            ScheduleGeneratorController testSchedGenCon = new ScheduleGeneratorController();
            JsonResult actual = testSchedGenCon.CoursesViewJson();
            string actualJson = new JavaScriptSerializer().Serialize(actual.Data);
            Assert.AreEqual(actualJson, programDirectorExpected);

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

            Assert.Equals(1, 1);
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
            string studentExpected = new JavaScriptSerializer().Serialize(new { Success = false, Message = "Please add one or more courses." });
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
            HomeController controller = new HomeController();


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


            string studentExpected = new JavaScriptSerializer().Serialize(new { Success = false, Message = "Please add a time option." });

            //Check if method generates appropriate schedule
            Assert.AreEqual(studentResult, studentExpected);

        }

        /*------------"GenerateScheduleModelWithCourses()" method testing------------------*/
        [TestMethod]
        public void VerifyGenerateScheduleModelWithVariousCourses()
        {

            List<Course> course = new List<Course>();

            ApplicationDbContext db = new ApplicationDbContext();

            Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == Season.Fall).FirstOrDefault(), EndTime = 1440 };
            List<Course> courses = db.Courses.ToList();
            preference.Courses = new List<Course>();
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 232 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 248 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 201 && n.CourseLetters == "ENGR").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 213 && n.CourseLetters == "ENGR").FirstOrDefault());
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };

            Program program = db.Program.FirstOrDefault();

            scheduleGenerator.GenerateSchedules(db.Enrollment.ToList());

            // We should expect more than one schedule
            Assert.IsTrue(scheduleGenerator.Schedules.Count() >= 1);

            // Test some other courses with another season
            preference = new Preference { Semester = db.Semesters.Where(n => n.Season == Season.Winter).FirstOrDefault(), EndTime = 1440 };
            preference.Courses = new List<Course>();
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 249 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 233 && n.CourseLetters == "ENGR").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 228 && n.CourseLetters == "SOEN").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 287 && n.CourseLetters == "SOEN").FirstOrDefault());
            scheduleGenerator = new ScheduleGenerator { Preference = preference };

            program = db.Program.FirstOrDefault();

            scheduleGenerator.GenerateSchedules(db.Enrollment.ToList());

            // We should expect more than one schedule
            Assert.IsTrue(scheduleGenerator.Schedules.Count() >= 1);

        }
        /*------------"GenerateScheduleHaveNoTimeConflict()" method testing------------------*/
        [TestMethod]
        public void VerifyGenerateScheduleHasNoTimeConflict()
        {
            // Generate a bunch of schedules and verify that none of them have a time conflict
            List<Course> course = new List<Course>();

            ApplicationDbContext db = new ApplicationDbContext();

            Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == Season.Fall).FirstOrDefault(), EndTime = 1440 };
            List<Course> courses = db.Courses.ToList();
            preference.Courses = new List<Course>();
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 232 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 248 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 201 && n.CourseLetters == "ENGR").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 213 && n.CourseLetters == "ENGR").FirstOrDefault());
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };

            Program program = db.Program.FirstOrDefault();

            scheduleGenerator.GenerateSchedules(db.Enrollment.ToList());

            // We should expect more than one schedule
            foreach (Schedule schedule in scheduleGenerator.Schedules.FirstOrDefault())
            {
                List<Section> sectionsInSchedule = new List<Section>();
                foreach (Enrollment enrollment in schedule.Enrollments)
                {
                    Assert.IsTrue(!scheduleGenerator.CheckIfTimeConflict(sectionsInSchedule, enrollment.Section));
                    sectionsInSchedule.Add(enrollment.Section);
                }
            }
        }
        /*------------"GenerateNoScheduleModelBecauseTimeConflict()" method testing------------------*/
        [TestMethod]
        public void VerifyGenerateNoPossibleSchedulesModel()
        {
            // We set the end time to 720 minutes, from functional testing and looking at the data we expect there to be no possible schedules with these constraints
            List<Course> course = new List<Course>();

            ApplicationDbContext db = new ApplicationDbContext();

            Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == Season.Fall).FirstOrDefault(), EndTime = 720 };
            List<Course> courses = db.Courses.ToList();
            preference.Courses = new List<Course>();
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 232 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 248 && n.CourseLetters == "COMP").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 201 && n.CourseLetters == "ENGR").FirstOrDefault());
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 213 && n.CourseLetters == "ENGR").FirstOrDefault());
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };

            Program program = db.Program.FirstOrDefault();

            scheduleGenerator.GenerateSchedules(db.Enrollment.ToList());

            // We should expect more than one schedule
            Assert.IsTrue(scheduleGenerator.Schedules.Count() == 0);

        }

        /*------------"GenerateNoScheduleModelBecauseNoCourseNotOfferedInSelectedSemester()" method testing------------------*/
        [TestMethod]
        public void VerifyGenerateNoPossibleSchedulesBecauseNoCourseOfferedModel()
        {
            // We try to generate a schedule with a course that isn't offered in the selected semester
            List<Course> course = new List<Course>();

            ApplicationDbContext db = new ApplicationDbContext();

            Preference preference = new Preference { Semester = db.Semesters.Where(n => n.Season == Season.Fall).FirstOrDefault(), EndTime = 720 };
            List<Course> courses = db.Courses.ToList();
            preference.Courses = new List<Course>();
            preference.Courses.Add(db.Courses.Where(n => n.CourseNumber == 228 && n.CourseLetters == "COMP").FirstOrDefault());
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = preference };

            Program program = db.Program.FirstOrDefault();

            scheduleGenerator.GenerateSchedules(db.Enrollment.ToList());

            // We should expect more than one schedule
            Assert.IsTrue(scheduleGenerator.Schedules.Count() == 0);

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
            List<string> courseCodes = new List<string> { "COMP 232", "COMP 248", "ENGR 201", "ENGR 213" };
            ViewResult view = testScheGenCon.GenerateSchedules(courseCodes, "Fall", null) as ViewResult;
            ScheduleGenerator schedGenerator = testScheGenCon.scheduleGenerator;

            testScheGenCon.sectionIds = new List<int>();

            Schedule scheduleToRegister = schedGenerator.Schedules.FirstOrDefault().FirstOrDefault();
            foreach (Enrollment enrollmentsInSchedule in scheduleToRegister.Enrollments)
                testScheGenCon.sectionIds.Add(enrollmentsInSchedule.Section.SectionId);

            testScheGenCon.RegisterSchedule();
            ApplicationUser user = db.Users.Where(n => n.Email == "harleymc@gmail.com").FirstOrDefault();
            Schedule registeredSchedule = db.Users.Where(n => n.Email == "harleymc@gmail.com").FirstOrDefault().Schedules.Where(n => n.Semester.SemesterID == scheduleToRegister.Semester.SemesterID).FirstOrDefault();

            // Check if the schedule from the db is the same that was generated and saved
            for (int i = 0; i < registeredSchedule.Enrollments.Count(); i++)
                Assert.IsTrue(registeredSchedule.Enrollments.ToList()[i].Section.SectionId == scheduleToRegister.Enrollments.ToList()[i].Section.SectionId);
        }





    }
}
