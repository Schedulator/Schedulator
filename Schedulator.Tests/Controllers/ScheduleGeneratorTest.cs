using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schedulator.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; 

namespace Schedulator.Tests.Controllers
{
    [TestClass]
    public class ScheduleGeneratorTest
    {
        [TestMethod]
        public void ScheduleGenerate()
        {
            List<Course> course = new List<Course>();
            
            ApplicationDbContext db = new ApplicationDbContext();
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator { Preference = new Preference { UseCourseSequence = true } };
            scheduleGenerator.GenerateSchedules(db.Courses.ToList(), db.Enrollment.ToList());
        }
    }
}
