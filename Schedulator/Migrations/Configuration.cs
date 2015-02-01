namespace Schedulator.Migrations
{
    using Schedulator.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Schedulator.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Schedulator.Models.ApplicationDbContext context)
        {
            context.Labs.ToList().ForEach(s => context.Labs.Remove(s));
            context.Tutorials.ToList().ForEach(s => context.Tutorials.Remove(s));
            context.Lectures.ToList().ForEach(s => context.Lectures.Remove(s));
            context.Courses.ToList().ForEach(s => context.Courses.Remove(s)); 

            var lectures = new List<Lecture>();
            var tutorials = new List<Tutorial>();
            var labs = new List<Lab>();
            var courses = new Course { Title = "BASIC CIRCUIT ANALYSIS", CourseLetters = "ELEC", CourseNumber = 273, SpecialNote = "Students who have received credit for ENGR 273 may not take this course for credit." };
            context.Courses.AddOrUpdate(courses);
            lectures.Add(new Lecture { Course = courses, LectureLetter = "R", Teacher = "Cowan, Glenn", StartTime = 10.15, EndTime = 11.30, FirstDay = "T", SecondDay = "J", ClassRoomNumber = "SGW H-531" });
            tutorials.Add(new Tutorial { Lecture = lectures.LastOrDefault(), TutorialLetter = "RA", StartTime = 14.30, EndTime = 16.10, FirstDay = "T", ClassRoomNumber = "SGW MB-5.215" });
            labs.Add(new Lab { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), LabLetter = "RI", StartTime = 17.45, EndTime = 20.30, FirstDay="M", ClassRoomNumber = "SGW H-822" });
            labs.Add(new Lab { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), LabLetter = "RJ", StartTime = 17.45, EndTime = 20.30, FirstDay = "M", ClassRoomNumber = "SGW H-822" });
            labs.Add(new Lab { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), LabLetter = "RK", StartTime = 15.30, EndTime = 18.15, FirstDay = "J", ClassRoomNumber = "SGW H-822" });
            tutorials.Add(new Tutorial { Lecture = lectures.LastOrDefault(), TutorialLetter = "RB", StartTime = 14.30, EndTime = 16.10, FirstDay = "M", ClassRoomNumber = "SGW FG-B070" });
            labs.Add(new Lab { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), LabLetter = "RL", StartTime = 14.45, EndTime = 17.25, FirstDay = "W", ClassRoomNumber = "SGW H-822" });
            labs.Add(new Lab { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), LabLetter = "RM", StartTime = 14.45, EndTime = 17.30, FirstDay = "W", ClassRoomNumber = "SGW H-822" });
            labs.Add(new Lab { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), LabLetter = "RB", StartTime = 14.45, EndTime = 17.30, FirstDay = "T", ClassRoomNumber = "SGW H-822" });

            lectures.ForEach(s => context.Lectures.AddOrUpdate(s));
            tutorials.ForEach(s => context.Tutorials.AddOrUpdate(s));
            labs.ForEach(s => context.Labs.AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}
