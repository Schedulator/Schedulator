using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schedulator.Models;
using System.Linq;
using Schedulator.Controllers;
using System.Web.Mvc; 
namespace Schedulator.Tests.Controllers
{
    [TestClass]
    public class CoursesControllerTest
    {
        [TestMethod]
        public void GetAllCourses()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            
            Assert.IsTrue(db.Courses.ToList().Count() > 120);

            db.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 228).ToList();

          

            CoursesController courseController = new CoursesController();

            // Act
            ViewResult result = courseController.Index() as ViewResult;
            result.AssertViewRendered().ForView("Index");


        }
    }
}
