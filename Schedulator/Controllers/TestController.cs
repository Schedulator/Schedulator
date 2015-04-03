﻿using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Schedulator.Controllers
{

    public class TestController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Test
        public void FirstYearStudent()
        {
            DoABunchOfShit(User.Identity.GetUserId(), 1, 2013);
        }

        public void SecondYearStudent()
        {
            DoABunchOfShit(User.Identity.GetUserId(), 2, 2014);
        }

        public void ThirdYearStudent()
        {
            DoABunchOfShit(User.Identity.GetUserId(), 3, 2015);
        }

        public void ForthYearStudent()
        {
            DoABunchOfShit(User.Identity.GetUserId(), 4, 2016);
        }

        public void DoABunchOfShit(string userId, int progressYear, int year)
        {
            List<Enrollment> studentEnrollments = db.Enrollment.Where(n => n.Schedule.ApplicationUser.Id == userId).ToList();

            Program studentsProgram = db.Users.Where(n => n.Id == userId).FirstOrDefault().Program;
            List<CourseSequence> courseSequencesToEnrollIn = studentsProgram.CourseSequences.Where(n => n.Year == progressYear).ToList();
            Schedule fallSchedule = new Schedule { Enrollments = new List<Enrollment>(), ApplicationUser = db.Users.Where(n => n.Id == userId).FirstOrDefault(), Semester = db.Semesters.Where(n => n.Season == Season.Fall && n.SemesterStart.Year == year).FirstOrDefault() };
            Schedule winterSchedule = new Schedule { Enrollments = new List<Enrollment>(), ApplicationUser = db.Users.Where(n => n.Id == userId).FirstOrDefault(), Semester = db.Semesters.Where(n => n.Season == Season.Winter && n.SemesterStart.Year == (year + 1)).FirstOrDefault() };
            Schedule summer1Schedule = new Schedule { Enrollments = new List<Enrollment>(), ApplicationUser = db.Users.Where(n => n.Id == userId).FirstOrDefault(), Semester = db.Semesters.Where(n => n.Season == Season.Summer1 && n.SemesterStart.Year == (year + 1)).FirstOrDefault() };
            Schedule summer2Schedule = new Schedule { Enrollments = new List<Enrollment>(), ApplicationUser = db.Users.Where(n => n.Id == userId).FirstOrDefault(), Semester = db.Semesters.Where(n => n.Season == Season.Summer2 && n.SemesterStart.Year == (year + 1)).FirstOrDefault() };
            foreach (CourseSequence courseSequence in courseSequencesToEnrollIn)
            {

                if (courseSequence.Season == Season.Fall)
                {
                    if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                        fallSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).FirstOrDefault() });
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                        fallSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).FirstOrDefault() });
                    else if (courseSequence.ElectiveType == ElectiveType.MathElective)
                        fallSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.MathElective).FirstOrDefault() });
                    else
                        fallSchedule.Enrollments.Add(new Enrollment { Course = courseSequence.Course, Grade = "A+" });
                }
                else if (courseSequence.Season == Season.Winter)
                {
                    if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                        winterSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).FirstOrDefault() });
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                        winterSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).FirstOrDefault() });
                    else if (courseSequence.ElectiveType == ElectiveType.MathElective)
                        winterSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.MathElective).FirstOrDefault() });
                    else
                        winterSchedule.Enrollments.Add(new Enrollment { Course = courseSequence.Course, Grade = "A+" });
                }
                else if (courseSequence.Season == Season.Summer1)
                {
                    if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                        summer1Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).FirstOrDefault() });
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                        summer1Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).FirstOrDefault() });
                    else if (courseSequence.ElectiveType == ElectiveType.MathElective)
                        summer1Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.MathElective).FirstOrDefault() });
                    else
                        summer1Schedule.Enrollments.Add(new Enrollment { Course = courseSequence.Course, Grade = "A+" });
                }
                else if (courseSequence.Season == Season.Summer2)
                {
                    if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                        summer2Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).FirstOrDefault() });
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                        summer2Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).FirstOrDefault() });
                    else if (courseSequence.ElectiveType == ElectiveType.MathElective)
                        summer2Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.MathElective).FirstOrDefault() });
                    else
                        summer2Schedule.Enrollments.Add(new Enrollment { Course = courseSequence.Course, Grade = "A+" });
                }
            }
            db.Schedule.Add(fallSchedule);
            db.Schedule.Add(winterSchedule);
            db.Schedule.Add(summer1Schedule);
            db.Schedule.Add(summer2Schedule);

            db.SaveChanges();
            studentEnrollments = db.Enrollment.Where(n => n.Schedule.ApplicationUser.Id == userId).ToList();
        }
    }
}