﻿using System;
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
    }
}
