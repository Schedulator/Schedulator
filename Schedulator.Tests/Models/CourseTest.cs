using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schedulator.Controllers;
using Schedulator.Models;

namespace Schedulator.Tests.Models
{
    [TestClass]
    public class CourseTest
    {
        [TestMethod]
        public void TestMissingPrerequisite()
        {
            //Create list oto hold "Enrollment" objects
            List<Enrollment> enrollments = new List<Enrollment>();
            
            //---------------------Create "Enrollment objects---------------------
            Schedule sched = new Schedule();
            Course course = new Course();
            Section section = new Section();
            Enrollment invalidEnrollmentID = new Enrollment{EnrollmentID = -1, Grade = "A", Schedule = sched, Course = course, Section = section}; //TODO should "Grade" be enum type?

            Course testCourse = new Course();

            //Store result of calling function with the above enrollments
            List<Prerequisite> result = new List<Prerequisite>();
            result = testCourse.MissingPrequisite(enrollments);

            //TODO: Assertion about function return value
            Assert.AreEqual(1, 1);
        }
    }
}
