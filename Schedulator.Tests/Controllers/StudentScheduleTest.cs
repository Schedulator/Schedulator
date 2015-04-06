using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schedulator.Models;
using Schedulator.Controllers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Schedulator.Tests.Controllers
{
    [TestClass]
    public class StudentScheduleTest
    {

        //-----------------------------------"GetSchedule()" method testing----------------------------------------

        [TestMethod]
        public void studentNullSemesterVerifyGetSchedule()
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

            //Create controller object, call "GetSchedule()" with null semester
            StudentScheduleController testController = new StudentScheduleController();
            ActionResult studentActual = testController.GetSchedule(null);

            //Exception that is expected to be thrown
            ArgumentNullException studentExpected = new ArgumentNullException("Please enter a semester.");

            //Check to make sure function threw appropriate error
            Assert.AreEqual(studentActual, studentExpected);
        }

         //------------------------------------"ManageSchedule()" method testing---------------------------------------
        [TestMethod]
        public void nullSectionIdsNullScheduleIdsVerifyManageSchedule()
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

            //Create controller object, call "GenerateSchedule()" with null semester
            StudentScheduleController testController = new StudentScheduleController();

            //Create schedule Ids for testing
            List<int> scheduleIds = new List<int>();
            scheduleIds.Add(db.Section.FirstOrDefault().SectionId);

            //Call function with null section Ids
            ActionResult nullSectionIdsResult = testController.ManageSchedule(scheduleIds, null, "remove");

            //Call function with null section Ids and null schedule IDs
            ActionResult nullSectionIdsNullScheduleIdsResult =  testController.ManageSchedule(null, null, "remove");

            Assert.Equals(nullSectionIdsNullScheduleIdsResult, nullSectionIdsResult);
        }

        //Test null schedule IDs that the database is unchanged
        [TestMethod]
        public void unchangedDatabaseVerifyManageSchedule()
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

            //Create schedule Ids for testing
            List<int> scheduleIds = new List<int>();
            scheduleIds.Add(db.Section.FirstOrDefault().SectionId);

            //Create controller object, call "ManageSchedule()" with null semester
            StudentScheduleController testController = new StudentScheduleController();

            //Number of elements in the database that have the specific section id before calling "ManageSchedule()"
            int countBefore = db.Schedule.Where(n => n.ScheduleId == db.Section.FirstOrDefault().SectionId).Count();
            
            testController.ManageSchedule(scheduleIds, null, "remove");

            //Number of elements in the database that have the specific section id after calling "ManageSchedule()"
            int countAfter = db.Schedule.Where(n => n.ScheduleId == db.Section.FirstOrDefault().SectionId).Count();

            Assert.AreEqual(countBefore, countAfter);

        }

        //------------------------------------"GenerateSchedules()" method testing---------------------------------------


        [TestMethod]
        public void studentNullSemesterVerifyGenerateSchedule()
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

            //Create controller object, call "GenerateSchedule()" with null semester
            StudentScheduleController testController = new StudentScheduleController();

            //Create course codes and sectionIds
            List<string> courseCodes = new List<string>{"COMP 248", "ENGR 201"};
            List<int> sectionIds = new List<int>();
            sectionIds.Add(db.Section.FirstOrDefault().SectionId);

            //Call the method with null semester 
            ActionResult studentActual = testController.GenerateSchedules(sectionIds, courseCodes, null);

            //Exception that is expected to be thrown
            ArgumentNullException studentExpected = new ArgumentNullException("Please enter a semester.");

            //Check to make sure function threw appropriate error
            Assert.AreEqual(studentActual, studentExpected);
        }


        

    }
}
