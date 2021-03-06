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
        public void DropAllSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();
            string user = db.Users.Find(User.Identity.GetUserId()).Email;
            schedules = db.Schedule.Where(t => t.ApplicationUser.Email == user).ToList();
            schedules.ForEach(n => db.Enrollment.RemoveRange(n.Enrollments));
            db.Schedule.RemoveRange(schedules);
            db.SaveChanges();
        }
        public void FirstYearStudent()
        {
            DoABunchOfShit(User.Identity.GetUserId(), 1, 2013);
        }

        public void SecondYearStudent()
        {
            DoABunchOfShit(User.Identity.GetUserId(), 1, 2012);
            DoABunchOfShit(User.Identity.GetUserId(), 2, 2013);
        }

        public void ThirdYearStudent()
        {
            DoABunchOfShit(User.Identity.GetUserId(), 1, 2011);
            DoABunchOfShit(User.Identity.GetUserId(), 2, 2012);
            DoABunchOfShit(User.Identity.GetUserId(), 3, 2013);
        }

        public void FourthYearStudent()
        {
            DoABunchOfShit(User.Identity.GetUserId(), 1, 2010);
            DoABunchOfShit(User.Identity.GetUserId(), 1, 2011);
            DoABunchOfShit(User.Identity.GetUserId(), 2, 2012);
            DoABunchOfShit(User.Identity.GetUserId(), 3, 2013);
        }
        public ActionResult ProgramList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Program.ToList());
        }
        public void ChangeProgram (int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            user.Program = db.Program.Where(n => n.ProgramId == id).FirstOrDefault();
            db.SaveChanges();
        }
        public string RandomGrade()
        {
            Random rnd = new Random();
            int number = rnd.Next(1, 9);
            switch (number)
            {
                case 1 : return "A+";
                case 2 : return "A";
                case 3 : return "A-";
                case 4 : return "B+";
                case 5 : return "B";
                case 6 : return "B-";
                case 7 : return "C+";
                case 8 : return "C";
                default : return "C-";
            }
        }
        public void DoABunchOfShit(string userId, int progressYear, int year)
        {
            List<Enrollment> studentEnrollments = db.Enrollment.Where(n => n.Schedule.ApplicationUser.Id == userId).ToList();

            Program studentsProgram = db.Users.Where(n => n.Id == userId).FirstOrDefault().Program;
            List<CourseSequence> courseSequencesToEnrollIn = studentsProgram.CourseSequences.Where(n => n.Year == progressYear && n.ContainerSequence == null).ToList();
            Schedule fallSchedule = new Schedule { Enrollments = new List<Enrollment>(), ApplicationUser = db.Users.Where(n => n.Id == userId).FirstOrDefault(), Semester = db.Semesters.Where(n => n.Season == Season.Fall && n.SemesterStart.Year == year).FirstOrDefault() };
            Schedule winterSchedule = new Schedule { Enrollments = new List<Enrollment>(), ApplicationUser = db.Users.Where(n => n.Id == userId).FirstOrDefault(), Semester = db.Semesters.Where(n => n.Season == Season.Winter && n.SemesterStart.Year == (year + 1)).FirstOrDefault() };
            Schedule summer1Schedule = new Schedule { Enrollments = new List<Enrollment>(), ApplicationUser = db.Users.Where(n => n.Id == userId).FirstOrDefault(), Semester = db.Semesters.Where(n => n.Season == Season.Summer1 && n.SemesterStart.Year == (year + 1)).FirstOrDefault() };
            Schedule summer2Schedule = new Schedule { Enrollments = new List<Enrollment>(), ApplicationUser = db.Users.Where(n => n.Id == userId).FirstOrDefault(), Semester = db.Semesters.Where(n => n.Season == Season.Summer2 && n.SemesterStart.Year == (year + 1)).FirstOrDefault() };
            foreach (CourseSequence courseSequence in courseSequencesToEnrollIn)
            {

                if (courseSequence.Season == Season.Fall)
                {
                    if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                        fallSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                        fallSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.ElectiveType == ElectiveType.MathElective)
                        fallSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.MathElective).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.OtherOptions.Count() > 0)
                    {
                        foreach (CourseSequence otherOptionsCourseSequence in courseSequence.OtherOptions)
                        {
                            Course courses = db.Courses.Where(n => n.CourseID == otherOptionsCourseSequence.Course.CourseID).FirstOrDefault();
                            if (courses != null)
                            {
                                fallSchedule.Enrollments.Add(new Enrollment { Course = courses, Grade = RandomGrade() });
                                break;
                            }
                        }
                    }
                    else
                        fallSchedule.Enrollments.Add(new Enrollment { Course = courseSequence.Course, Grade = RandomGrade() });
                }
                else if (courseSequence.Season == Season.Winter)
                {
                    if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                        winterSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                        winterSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.ElectiveType == ElectiveType.MathElective)
                        winterSchedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.MathElective).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.OtherOptions.Count() > 0)
                    {
                        foreach (CourseSequence otherOptionsCourseSequence in courseSequence.OtherOptions)
                        {
                            Course courses = db.Courses.Where(n => n.CourseID == otherOptionsCourseSequence.Course.CourseID).FirstOrDefault();
                            if (courses != null)
                            {
                                winterSchedule.Enrollments.Add(new Enrollment { Course = courses, Grade = RandomGrade() });
                                break;
                            }
                        }
                    }
                    else
                        winterSchedule.Enrollments.Add(new Enrollment { Course = courseSequence.Course, Grade = RandomGrade() });
                }
                else if (courseSequence.Season == Season.Summer1)
                {
                    if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                        summer1Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                        summer1Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.ElectiveType == ElectiveType.MathElective)
                        summer1Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.MathElective).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.OtherOptions.Count() > 0)
                    {
                        foreach (CourseSequence otherOptionsCourseSequence in courseSequence.OtherOptions)
                        {
                            Course courses = db.Courses.Where(n => n.CourseID == otherOptionsCourseSequence.Course.CourseID).FirstOrDefault();
                            if (courses != null)
                            {
                                summer1Schedule.Enrollments.Add(new Enrollment { Course = courses, Grade = RandomGrade() });
                                break;
                            }
                        }
                    }
                    else
                        summer1Schedule.Enrollments.Add(new Enrollment { Course = courseSequence.Course, Grade = RandomGrade() });
                }
                else if (courseSequence.Season == Season.Summer2)
                {
                    if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                        summer2Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                        summer2Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).FirstOrDefault(), Grade = RandomGrade() });
                    else if (courseSequence.ElectiveType == ElectiveType.MathElective)
                        summer2Schedule.Enrollments.Add(new Enrollment { Course = db.Courses.Where(n => n.ElectiveType == ElectiveType.MathElective).FirstOrDefault() });
                    else if (courseSequence.OtherOptions.Count() > 0)
                    {
                        foreach (CourseSequence otherOptionsCourseSequence in courseSequence.OtherOptions)
                        {
                            Course courses = db.Courses.Where(n => n.CourseID == otherOptionsCourseSequence.Course.CourseID).FirstOrDefault();
                            if (courses != null)
                            {
                                summer2Schedule.Enrollments.Add(new Enrollment { Course = courses, Grade = RandomGrade() });
                                break;
                            }
                        }
                    }
                    else
                        summer2Schedule.Enrollments.Add(new Enrollment { Course = courseSequence.Course, Grade = RandomGrade() });
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