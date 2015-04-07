namespace Schedulator.Migrations
{
    using Excel;
    using Schedulator.Controllers;
    using Schedulator.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.Owin.Host.SystemWeb;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Validation;

    internal sealed class Configuration : DbMigrationsConfiguration<Schedulator.Models.ApplicationDbContext>
    {
        List<Course> courses = new List<Course>();
        List<Lecture> lectures = new List<Lecture>();
        List<Tutorial> tutorials = new List<Tutorial>();
        List<Lab> labs = new List<Lab>();
        List<Prerequisite> prerequisites = new List<Prerequisite>();
        List<Section> sections = new List<Section>();

        Semester fallSemester;
        Semester winterSemester;
        Semester summerOneSemester;
        Semester summerTwoSemester;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Schedulator.Models.ApplicationDbContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false) // Uncomment if you want to debug
                System.Diagnostics.Debugger.Launch();
            string currentDirectoryUrl = Directory.GetCurrentDirectory();


            foreach (ApplicationUser user in context.Users.ToList())
            {
                user.Program = null;
                context.Users.AddOrUpdate(user);
            }
            SeedSemester(context);
            SeedCoursesFromExcelSheet(context);
            SeedProgramsFromExcelSheet(context);
            AddScienceElectives(context);
            AddGeneralElective(context);
            AddTechnicalElectives(context);
            //Schedule schedule = new Schedule { ApplicationUser = context.Users.Where(u => u.Email == "harleymc@gmail.com").FirstOrDefault(), Semester = context.Semesters.Where(n => n.Season == Season.Fall).FirstOrDefault() , IsRegisteredSchedule = true };
            //AddPrerequisite(context);



            //List<Enrollment> enrollments = new List<Enrollment>();

            ////enrollments.Add(new Enrollment { Schedule = schedule, Section = context.Section.Where(t => t.Tutorial.TutorialLetter == "QB" && t.Lecture.Course.CourseLetters == "COMP" && t.Lecture.Course.CourseNumber == 232).FirstOrDefault(), Grade = "B-" });
            ////enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            ////enrollments.Add(new Enrollment { Schedule = schedule, Section = context.Section.Where(t => t.Tutorial.TutorialLetter == "AE" && t.Lecture.Course.CourseLetters == "COMP" && t.Lecture.Course.CourseNumber == 248).FirstOrDefault(), Grade = "A" });
            ////enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            ////enrollments.Add(new Enrollment { Schedule = schedule, Section = context.Section.Where(t => t.Lecture.Course.CourseLetters == "ENGR" && t.Lecture.Course.CourseNumber == 201 && t.Tutorial.TutorialLetter == "GA").FirstOrDefault(), Grade = "B+" });
            ////enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            ////enrollments.Add(new Enrollment { Schedule = schedule, Section = context.Section.Where(t => t.Lecture.Course.CourseLetters == "ENGR" && t.Lecture.Course.CourseNumber == 213 && t.Tutorial.TutorialLetter == "PA").FirstOrDefault(), Grade = "B+" });
            ////enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            ////context.Schedule.AddOrUpdate(schedule);
            
            ////enrollments.ForEach(p => context.Enrollment.AddOrUpdate(p));

            ////context.SaveChanges();

            //schedule = new Schedule { ApplicationUser = context.Users.Where(u => u.FirstName == "Harrison").FirstOrDefault(), Semester = context.Semesters.Where(p => p.Season == Season.Fall).First(), IsRegisteredSchedule = true };

            //enrollments = new List<Enrollment>();

            //enrollments.Add(new Enrollment { Schedule = schedule, Section = context.Section.Where(t => t.Tutorial.TutorialLetter == "QB" && t.Lecture.Course.CourseLetters == "COMP" && t.Lecture.Course.CourseNumber == 232).FirstOrDefault(), Grade = "B-" });
            //enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            //enrollments.Add(new Enrollment { Schedule = schedule, Section = context.Section.Where(t => t.Tutorial.TutorialLetter == "AE" && t.Lecture.Course.CourseLetters == "COMP" && t.Lecture.Course.CourseNumber == 248).FirstOrDefault(), Grade = "A" });
            //enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            //enrollments.Add(new Enrollment { Schedule = schedule, Section = context.Section.Where(t => t.Lecture.Course.CourseLetters == "ENGR" && t.Lecture.Course.CourseNumber == 201 && t.Tutorial.TutorialLetter == "GA").FirstOrDefault(), Grade = "B+" });
            //enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            //enrollments.Add(new Enrollment { Schedule = schedule, Section = context.Section.Where(t => t.Lecture.Course.CourseLetters == "ENGR" && t.Lecture.Course.CourseNumber == 213 && t.Tutorial.TutorialLetter == "PA").FirstOrDefault(), Grade = "B+" });
            //enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            //context.Schedule.AddOrUpdate(schedule);

            //enrollments.ForEach(p => context.Enrollment.AddOrUpdate(p));
            
            context.SaveChanges();
        }

        private void AddTechnicalElectives(ApplicationDbContext context)
        {
            List<Program> compGamesProgramList = context.Program.Where(n => n.ProgramOption == "Computer Games").ToList();
            List<Program> realTimeProgramList = context.Program.Where(n => n.ProgramOption == "Real-time Embedded and Avionics Software").ToList();
            List<Program> webServicesProgramList = context.Program.Where(n => n.ProgramOption == "Web Services & Applications").ToList();
            List<Program> generalProgramList = context.Program.Where(n => n.ProgramOption == "General Program").ToList();

            // Add all Computer Games technical elective courses to all Computer Games programs
            foreach (Program program in compGamesProgramList)
            {
                program.TechnicalElectiveCourses = new List<Course>();
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 345).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 353).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 371).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 376).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 472).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 476).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 477).FirstOrDefault());
            }

            foreach (Program program in compGamesProgramList)
            {
                context.Entry(program).State = EntityState.Modified;
            }
            context.SaveChanges();

            // Add all Real Time technical elective courses to all Real Time programs
            foreach (Program program in realTimeProgramList)
            {
                program.TechnicalElectiveCourses = new List<Course>();
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "AERO" && n.CourseNumber == 480).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "AERO" && n.CourseNumber == 482).FirstOrDefault());

                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COEN" && n.CourseNumber == 320).FirstOrDefault());

                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 345).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 444).FirstOrDefault());

                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 422).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 423).FirstOrDefault());
            }

            foreach (Program program in realTimeProgramList)
            {
                context.Entry(program).State = EntityState.Modified;
            }
            context.SaveChanges();

            // Add all Web Services technical elective courses to all Web Services programs
            foreach (Program program in webServicesProgramList)
            {
                program.TechnicalElectiveCourses = new List<Course>();
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 353).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 445).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 479).FirstOrDefault());

                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 387).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 487).FirstOrDefault());
            }

            foreach (Program program in webServicesProgramList)
            {
                context.Entry(program).State = EntityState.Modified;
            }
            context.SaveChanges();

            // Add all General Program technical elective courses to General Program programs
            foreach (Program program in generalProgramList)
            {
                program.TechnicalElectiveCourses = new List<Course>();
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 345).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 353).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 371).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 426).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 428).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 442).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 445).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 451).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 465).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 472).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 473).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 474).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 478).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 479).FirstOrDefault());

                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 298).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 422).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 423).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 448).FirstOrDefault());
                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "SOEN" && n.CourseNumber == 491).FirstOrDefault());

                program.TechnicalElectiveCourses.Add(context.Courses.Where(n => n.CourseLetters == "ENGR" && n.CourseNumber == 411).FirstOrDefault());
            }

            foreach (Program program in generalProgramList)
            {
                context.Entry(program).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
        void SeedProgramsFromExcelSheet(ApplicationDbContext context)
        {
            context.CourseSequence.ToList().ForEach(s => context.CourseSequence.Remove(s));
            context.Program.ToList().ForEach(s => context.Program.Remove(s));
            context.SaveChanges();
            
            List<Program> programs = AddProgramAndCourseSequence(@"C:\Users\Harley\Desktop\Schedulator\Schedulator\Programs.xlsx", context.Courses.ToList());

            programs.ForEach(p => context.Program.Add(p));
            programs.ForEach(p => context.CourseSequence.AddRange(p.CourseSequences));

            foreach (ApplicationUser user in context.Users.ToList())
            {
                user.Program = programs.First();
                context.Users.AddOrUpdate(user);
            }
            context.SaveChanges();
        }
        void SeedSemester(ApplicationDbContext context)
        {

            context.Semesters.Add(new Semester { Season = Season.Fall, SemesterStart = new DateTime(2011, 9, 1), SemesterEnd = new DateTime(2011, 12, 18) });
            context.Semesters.Add(new Semester { Season = Season.Winter, SemesterStart = new DateTime(2012, 1, 7), SemesterEnd = new DateTime(2012, 5, 2) });
            context.Semesters.Add(new Semester { Season = Season.Summer1, SemesterStart = new DateTime(2012, 5, 4), SemesterEnd = new DateTime(2012, 6, 23) });
            context.Semesters.Add(new Semester { Season = Season.Summer2, SemesterStart = new DateTime(2012, 6, 25), SemesterEnd = new DateTime(2012, 7, 19) });

            context.Semesters.Add(new Semester { Season = Season.Fall, SemesterStart = new DateTime(2012, 9, 1), SemesterEnd = new DateTime(2012, 12, 18) });
            context.Semesters.Add(new Semester { Season = Season.Winter, SemesterStart = new DateTime(2013, 1, 7), SemesterEnd = new DateTime(2013, 5, 2) });
            context.Semesters.Add(new Semester { Season = Season.Summer1, SemesterStart = new DateTime(2013, 5, 4), SemesterEnd = new DateTime(2013, 6, 23) });
            context.Semesters.Add(new Semester { Season = Season.Summer2, SemesterStart = new DateTime(2013, 6, 25), SemesterEnd = new DateTime(2013, 7, 19) });

            context.Semesters.Add(new Semester { Season = Season.Fall, SemesterStart = new DateTime(2013, 9, 1), SemesterEnd = new DateTime(2013, 12, 18) });
            context.Semesters.Add(new Semester { Season = Season.Winter, SemesterStart = new DateTime(2014, 1, 7), SemesterEnd = new DateTime(2014, 5, 2) });
            context.Semesters.Add(new Semester { Season = Season.Summer1, SemesterStart = new DateTime(2014, 5, 4), SemesterEnd = new DateTime(2014, 6, 23) });
            context.Semesters.Add(new Semester { Season = Season.Summer2, SemesterStart = new DateTime(2014, 6, 25), SemesterEnd = new DateTime(2014, 7, 19) });

            context.SaveChanges();
        }
        void SeedCoursesFromExcelSheet(ApplicationDbContext context)
        {
            context.CourseSequence.ToList().ForEach(s => context.CourseSequence.Remove(s));
            context.Program.ToList().ForEach(s => context.Program.Remove(s));
            context.Prerequisite.ToList().ForEach(s => context.Prerequisite.Remove(s));
            context.Enrollment.ToList().ForEach(s => context.Enrollment.Remove(s));
            context.Section.ToList().ForEach(s => context.Section.Remove(s));
            context.Schedule.ToList().ForEach(s => context.Schedule.Remove(s));
            context.Labs.ToList().ForEach(s => context.Labs.Remove(s));
            context.Tutorials.ToList().ForEach(s => context.Tutorials.Remove(s));
            context.Lectures.ToList().ForEach(s => context.Lectures.Remove(s));
            context.Courses.ToList().ForEach(s => context.Courses.Remove(s));
            context.Semesters.ToList().ForEach(s => context.Semesters.Remove(s));

            context.SaveChanges();

            if (!context.Roles.Any(u => u.Name == "Student"))
                context.Roles.AddOrUpdate(new IdentityRole { Name = "Student" });
            if (!context.Roles.Any(u => u.Name == "Program Director"))
                context.Roles.AddOrUpdate(new IdentityRole { Name = "Program Director" });

            context.SaveChanges();

            if (!context.Users.Any(u => u.UserName == "harley.1011@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { FirstName = "Harley", LastName = "McPhee", UserName = "harley.1011@gmail.com" };

                var result = manager.Create(user, "Password@123!");
                manager.AddToRole(user.Id, "Student");

            }



            fallSemester = new Semester { Season = Season.Fall, SemesterStart = new DateTime(2014, 9, 1), SemesterEnd = new DateTime(2014, 12, 18) };
            winterSemester = new Semester { Season = Season.Winter, SemesterStart = new DateTime(2015, 1, 7), SemesterEnd = new DateTime(2015, 5, 2) };
            summerOneSemester = new Semester { Season = Season.Summer1, SemesterStart = new DateTime(2015, 5, 4), SemesterEnd = new DateTime(2015, 6, 23) };
            summerTwoSemester = new Semester { Season = Season.Summer2, SemesterStart = new DateTime(2015, 6, 25), SemesterEnd = new DateTime(2015, 7, 19) };

            context.Semesters.AddOrUpdate(fallSemester);
            context.Semesters.AddOrUpdate(winterSemester);
            context.Semesters.AddOrUpdate(summerOneSemester);
            context.Semesters.AddOrUpdate(summerTwoSemester);


            foreach (var worksheet in Workbook.Worksheets(@"C:\Users\Harley\Desktop\Schedulator\Schedulator\SoftwareAndCompCourseList.xlsx"))
            {
                var row = worksheet.Rows;
                int count = 0;

                int max = row.Count();
                while (count < row.Count())
                {
                    string currentCell = row[count].Cells[0].Text;


                    if (currentCell != null & Regex.IsMatch(currentCell, @"[A-Z]{4}\s\d{3}"))
                    {
                        double credit = 0;
                        if (Regex.IsMatch((row[count].Cells[4].Text), @"\d*"))
                            credit = Convert.ToDouble(Regex.Match(row[count].Cells[4].Text, @"\d*").ToString());
                        courses.Add(new Course { CourseLetters = Regex.Match(currentCell, @"[A-Z]{4}").ToString(), CourseNumber = Convert.ToInt32(Regex.Match(currentCell, @"\d{3}").ToString()), Credit = credit,  Title = row[count].Cells[1].Text });
                        

                        System.Diagnostics.Debug.WriteLine(courses.LastOrDefault().CourseLetters + courses.LastOrDefault().CourseNumber);
                        count++;
                        if (count < row.Count() && row[count].Cells[0] != null && row[count].Cells[0].Text == "Prerequisite:")
                        {
                            count++;
                        }
                        if (count < row.Count() && row[count].Cells[0] != null && row[count].Cells[0].Text == "Special Note:")
                        {
                            courses.LastOrDefault().SpecialNote = row[count].Cells[1].Text;
                            count++;
                        }
                        while (count < row.Count() && row[count].Cells[0] != null && (row[count].Cells[0].Text == "Fall" || row[count].Cells[0].Text == "Winter" || row[count].Cells[0].Text == "Summer"))
                        {
                            Semester temp = null;

                            if (count < row.Count() && row[count].Cells[0].Text == "Fall")
                            {
                                temp = fallSemester;
                                count++;
                            }
                            else if (count < row.Count() && row[count].Cells[0].Text == "Winter")
                            {
                                temp = winterSemester;
                                count++;
                            }
                            else if (count < row.Count() && row[count].Cells[0].Text == "Summer" && (Regex.IsMatch(row[count].Cells[1].Text, "May")))
                            {
                                temp = summerOneSemester;
                                count++;
                            }
                            else if (count < row.Count() && row[count].Cells[0].Text == "Summer")
                            {
                                temp = summerTwoSemester;
                                count++;
                            }
                            else
                                count++;
                            while (count < row.Count() && row[count].Cells[1] != null && (Regex.IsMatch(row[count].Cells[1].Text, @"Lect\s[A-Z]{1,2}") || row[count].Cells[1].Text == "UgradNSched IE"))
                            {
                                lectures.Add(new Lecture { Course = courses.LastOrDefault(), Semester = temp });
                                if (count < row.Count() && row[count].Cells[1] != null && (row[count].Cells[1].Text == "UgradNSched IE"))
                                {
                                    lectures.LastOrDefault().LectureLetter = "UgradNSched IE";
                                    time timeToSet = ParseTime(row[count].Cells[2].Text);

                                    lectures.LastOrDefault().StartTime = 0; ;
                                    lectures.LastOrDefault().EndTime = 0;
                                    count++;
                                }
                                else if (count < row.Count() && row[count].Cells[1] != null && Regex.IsMatch(row[count].Cells[1].Text, @"Lect\s[A-Z]{1,2}"))
                                {
                                    string value = Regex.Match(row[count].Cells[1].Text, @"[A-Z]{2}").Value;
                                    lectures.LastOrDefault().LectureLetter = Regex.Replace(row[count].Cells[1].Text, @"Lect\s", "");
                                    time timeToSet = ParseTime(row[count].Cells[2].Text);

                                    lectures.LastOrDefault().StartTime = timeToSet.startTime;
                                    lectures.LastOrDefault().EndTime = timeToSet.endTime;
                                    lectures.LastOrDefault().FirstDay = timeToSet.firstDay;
                                    lectures.LastOrDefault().SecondDay = timeToSet.secondDay;
                                    lectures.LastOrDefault().ClassRoomNumber = row[count].Cells[3].Text;
                                    lectures.LastOrDefault().Teacher = row[count].Cells[4].Text;
                                    count++;


                                    while (count < row.Count() && row[count].Cells[1] != null && Regex.IsMatch(row[count].Cells[1].Text, @"Tut\s[A-Z]{1,2}"))
                                    {
                                        tutorials.Add(new Tutorial { Lecture = lectures.LastOrDefault(), TutorialLetter = Regex.Replace(row[count].Cells[1].Text, @"Tut\s", "") });
                                        timeToSet = ParseTime(row[count].Cells[2].Text);

                                        tutorials.LastOrDefault().StartTime = timeToSet.startTime;
                                        tutorials.LastOrDefault().EndTime = timeToSet.endTime;
                                        tutorials.LastOrDefault().FirstDay = timeToSet.firstDay;
                                        tutorials.LastOrDefault().SecondDay = timeToSet.secondDay;
                                        tutorials.LastOrDefault().ClassRoomNumber = row[count].Cells[3].Text;

                                        count++;
                                        if (!(count < row.Count() && row[count].Cells[1] != null && Regex.IsMatch(row[count].Cells[1].Text, @"Lab\s([A-Z]|\d){1,2}")))
                                        {
                                            Section section = new Section { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault() };
                                            CheckIfSectionHasSameTimes(section);
                                            sections.Add(section);
                                        }

                                        while (count < row.Count() && row[count].Cells[1] != null && Regex.IsMatch(row[count].Cells[1].Text, @"Lab\s([A-Z]|\d){1,2}"))
                                        {
                                            labs.Add(new Lab { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), LabLetter = Regex.Replace(row[count].Cells[1].Text, @"Lab\s", "") });
                                            timeToSet = ParseTime(row[count].Cells[2].Text);

                                            labs.LastOrDefault().StartTime = timeToSet.startTime;
                                            labs.LastOrDefault().EndTime = timeToSet.endTime;
                                            labs.LastOrDefault().FirstDay = timeToSet.firstDay;
                                            labs.LastOrDefault().SecondDay = timeToSet.secondDay;
                                            labs.LastOrDefault().ClassRoomNumber = row[count].Cells[3].Text;

                                            count++;
                                            Section section = new Section { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), Lab = labs.LastOrDefault() };
                                            CheckIfSectionHasSameTimes(section);
                                            sections.Add(section);

                                            // lectures.LastOrDefault().Sections.Add(sections.LastOrDefault());
                                        }
                                    }
                                    while (count < row.Count() && row[count].Cells[1] != null && Regex.IsMatch(row[count].Cells[1].Text, @"Lab\s([A-Z]|\d){1,2}"))
                                    {
                                        labs.Add(new Lab { Lecture = lectures.LastOrDefault(), LabLetter = Regex.Replace(row[count].Cells[1].Text, @"Lab\s", "") });
                                        timeToSet = ParseTime(row[count].Cells[2].Text);

                                        labs.LastOrDefault().StartTime = timeToSet.startTime;
                                        labs.LastOrDefault().EndTime = timeToSet.endTime;
                                        labs.LastOrDefault().FirstDay = timeToSet.firstDay;
                                        labs.LastOrDefault().SecondDay = timeToSet.secondDay;
                                        labs.LastOrDefault().ClassRoomNumber = row[count].Cells[3].Text;

                                        count++;
                                        Section section = new Section { Lecture = lectures.LastOrDefault(), Lab = labs.LastOrDefault() };
                                        CheckIfSectionHasSameTimes(section);
                                        sections.Add(section);
                                        //  lectures.LastOrDefault().Sections.Add(sections.LastOrDefault());
                                    }
                                    if (sections.LastOrDefault().Lecture != lectures.LastOrDefault())
                                    {
                                        Section section = new Section { Lecture = lectures.LastOrDefault() };
                                        CheckIfSectionHasSameTimes(section);
                                        sections.Add(section);
                                    }

                                }
                            }
                        }


                    }
                    else
                        count++;

                }

            }
            courses.ForEach(p => context.Courses.AddOrUpdate(p));
            lectures.ForEach(p => context.Lectures.AddOrUpdate(p));
            tutorials.ForEach(p => context.Tutorials.AddOrUpdate(p));
            labs.ForEach(p => context.Labs.AddOrUpdate(p));
            sections.ForEach(p => context.Section.AddOrUpdate(p));
            context.SaveChanges();
        }
        List<Program> AddProgramAndCourseSequence (string url, List<Course> courses)
        {
            List<Program> programs = new List<Program>();
            
            int counter = 0;
            foreach (var worksheet in Workbook.Worksheets(url))
            {
                List<CourseSequence> courseSequences = new List<CourseSequence>();
                counter++;
                int count = 1;
                var rows = worksheet.Rows;
                programs.Add(new Program { ProgramName = rows[0].Cells[0].Text, ProgramOption = rows[0].Cells[1].Text, ProgramSemester = rows[0].Cells[2].Text, CreditsRequirement = Convert.ToInt32(rows[0].Cells[3].Text) });
                System.Diagnostics.Debug.WriteLine(rows[0].Cells[0].Text + " " + rows[0].Cells[1].Text + " " + rows[0].Cells[2].Text);
                if (rows[0].Cells[1].Text == "Software System")
                    Console.WriteLine("h");
                Season season = 0;
                int year = 0;
                while (count < rows.Count())
                {
                    string currentCellText = rows[count].Cells[0].Text;
                    if (count == 39)
                    {
                        Console.WriteLine("h");
                    }
                    if (Regex.IsMatch(currentCellText, @"Year\s\d{1}\s(Winter|Fall|Summer)"))
                    {
                        year = Convert.ToInt32(Regex.Match(currentCellText, @"\d{1}").ToString());
                        string temp = Regex.Match(currentCellText, @"(Fall|Winter|Summer)").ToString();
                        if (Regex.IsMatch(currentCellText, @"Year\s\d{1}\s(Summer)"))
                            season = Season.Summer1;
                        else
                            season = (Season)Enum.Parse(typeof(Season), Regex.Match(currentCellText, @"(Fall|Winter)").ToString());
                    }
                    else if ( Regex.IsMatch(currentCellText, @"[A-Z]{4}\s\d{3} or"))
                    {
                        
                        List<CourseSequence> otherCourseOptions = new List<CourseSequence>();
                        otherCourseOptions.Add(new CourseSequence { Program = programs.LastOrDefault(), OtherOptions = new List<CourseSequence>(),  Year = year, Season = season });
                        ElectiveType electiveType = ElectiveType.None;
                       // if (currentCellText.Contains("Elective"))
                           // electiveType = ElectiveType.TechnicalElective;
                        currentCellText = rows[count].Cells[0].Text;
                        string mergeRows = "";
                        while (Regex.IsMatch(currentCellText,@"([A-Z]{4}\s\d{3} or|Elective)"))
                        {
                            if (Regex.IsMatch(currentCellText, @"([A-Z]{4}\s\d{3} or Elective)"))
                            {
                                mergeRows += Regex.Match(currentCellText, @"[A-Z]{4}\s\d{3} or").ToString();
                                mergeRows += "Elective";
                                count++;
                                break;
                            }
                            else
                            {
                                mergeRows += Regex.Match(currentCellText, @"([A-Z]{4}\s\d{3} or|Elective)").ToString();
                                currentCellText = rows[++count].Cells[0].Text;
                            }
                        }

                        foreach (Match match in Regex.Matches(mergeRows, @"([A-Z]{4}\s\d{3}|Elective)"))
                        {
                            CourseSequence temp;
                            if ( match.ToString() != "Elective")
                                 temp = new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = electiveType, Year = year, Season = season, Course = courses.Where(p => p.CourseLetters == Regex.Match(match.ToString(), @"[A-Z]{4}").ToString() && p.CourseNumber == Convert.ToInt32(Regex.Match(match.ToString(), @"\d{3}").ToString())).FirstOrDefault() };
                            else
                                temp = new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = ElectiveType.TechnicalElective, Year = year, Season = season};
                            otherCourseOptions.Add(temp);
                            otherCourseOptions.First().OtherOptions.Add(temp);
                            temp.ContainerSequence = otherCourseOptions.First();
                        }
                        courseSequences.AddRange(otherCourseOptions);
                    }
                    else if ( Regex.IsMatch(currentCellText, @"[A-Z]{4}\s\d{3}"))
                        courseSequences.Add(new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = Models.ElectiveType.None, Year = year, Season = season, Course = courses.Where(p => p.CourseLetters == Regex.Match(currentCellText, @"[A-Z]{4}").ToString() && p.CourseNumber == Convert.ToInt32(Regex.Match(currentCellText, @"\d{3}").ToString())).FirstOrDefault() });
                    else if (currentCellText.Contains("General Elective"))
                        courseSequences.Add(new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = Models.ElectiveType.GeneralElective, Year = year, Season = season });
                    else if (currentCellText.Contains("Basic Science"))
                        courseSequences.Add(new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = Models.ElectiveType.BasicScience, Year = year, Season = season });
                    else if (currentCellText.Contains("Elective") || currentCellText.Contains("Technical Elective"))
                    {
                        if (programs.LastOrDefault().ProgramName != "Computer Science")
                            courseSequences.Add(new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = Models.ElectiveType.TechnicalElective, Year = year, Season = season });
                        else
                            courseSequences.Add(new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = Models.ElectiveType.GeneralElective, Year = year, Season = season });
                    }
                count++;

                }
                programs.LastOrDefault().CourseSequences = courseSequences;
            }
            return programs;
        }
        void CheckIfSectionHasSameTimes(Section section)
        {

            List<Section> sameTimeSections =  sections.Where(n => n.Lecture.Course == section.Lecture.Course && n.Lecture.Semester == section.Lecture.Semester && n.Lecture.FirstDay == section.Lecture.FirstDay && n.Lecture.SecondDay == section.Lecture.SecondDay && n.Lecture.StartTime == section.Lecture.StartTime && n.Lecture.EndTime == section.Lecture.EndTime).ToList();
            if (sameTimeSections.Count > 0 && section.Tutorial != null)
                sameTimeSections = sections.Where(n => n.Lecture.Course == section.Lecture.Course && n.Lecture.Semester == section.Lecture.Semester && n.Lecture.FirstDay == section.Lecture.FirstDay && n.Lecture.SecondDay == section.Lecture.SecondDay && n.Lecture.StartTime == section.Lecture.StartTime && n.Lecture.EndTime == section.Lecture.EndTime && n.Lecture.Semester == section.Lecture.Semester && n.Lecture.Course == section.Lecture.Course && n.Tutorial.FirstDay == section.Tutorial.FirstDay && n.Tutorial.SecondDay == section.Tutorial.SecondDay && n.Tutorial.StartTime == section.Tutorial.StartTime && n.Tutorial.EndTime == section.Tutorial.EndTime).ToList();
            if (sameTimeSections.Count > 0 && section.Lab != null)
                sameTimeSections = sections.Where(n => n.Lecture.Course == section.Lecture.Course && n.Lecture.Semester == section.Lecture.Semester && n.Lecture.FirstDay == section.Lecture.FirstDay && n.Lecture.SecondDay == section.Lecture.SecondDay && n.Lecture.StartTime == section.Lecture.StartTime && n.Lecture.EndTime == section.Lecture.EndTime && n.Lecture.Course == section.Lecture.Course && n.Lab.FirstDay == section.Lab.FirstDay && n.Lab.SecondDay == section.Lab.SecondDay && n.Lab.StartTime == section.Lab.StartTime && n.Lab.EndTime == section.Lab.EndTime).ToList();
            
            if (sameTimeSections.Count > 0)
            {
                Section sectionMaster = sameTimeSections.Where(n => n.SectionMaster).FirstOrDefault();
                if (sectionMaster == null)
                {
                    sameTimeSections.FirstOrDefault().SectionMaster = true;
                    sameTimeSections.FirstOrDefault().OtherSimilarSections = new List<Section>();
                    section.OtherSimilarSectionMaster = sameTimeSections.FirstOrDefault();
                }
                else
                {
                    sectionMaster.OtherSimilarSections.Add(section);
                    section.OtherSimilarSectionMaster = sectionMaster;
                }
            }
        }
        void AddGeneralElective (ApplicationDbContext context)
        {
            List<Course> generalCourses = new List<Course>();
            List<Lecture> generalLectures = new List<Lecture>();

            generalCourses.Add(new Course { CourseLetters = "ADMI", CourseNumber = 201, Credit = 3, ElectiveType = ElectiveType.GeneralElective, Title = "INTRO TO ADMINISTRATION" });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.270", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1050, Semester = fallSemester, LectureLetter = "A", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-3.270", FirstDay = TimeBlock.day.J, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1050, Semester = winterSemester, LectureLetter = "B", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.430", FirstDay = TimeBlock.day.J, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1050, Semester = winterSemester, LectureLetter = "AA", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });


            generalCourses.Add(new Course { CourseLetters = "ADMI", CourseNumber = 202, Credit = 3, ElectiveType = ElectiveType.GeneralElective, Title = "PERSPECTIVE ON CANADIAN BUS" });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-5.270", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1050, Semester = fallSemester, LectureLetter = "A", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-5.270", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1050, Semester = winterSemester, LectureLetter = "B", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });


            generalCourses.Add(new Course { CourseLetters = "MANA", CourseNumber = 201, Credit = 3, ElectiveType = ElectiveType.GeneralElective, Title = "INTRO BUSINESS & MANAGEMENT" });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.210", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 1110, EndTime = 1260, Semester = summerOneSemester, LectureLetter = "AA", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-5.210", FirstDay = TimeBlock.day.J, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = fallSemester, LectureLetter = "BB", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-5.210", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = fallSemester, LectureLetter = "AA", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-5.210", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = winterSemester, LectureLetter = "CC", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });

            generalCourses.Add(new Course { CourseLetters = "MANA", CourseNumber = 202, Credit = 3, ElectiveType = ElectiveType.GeneralElective, Title = "HUMAN BEHAVIOUR IN ORGS" });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.270", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = fallSemester, LectureLetter = "AA", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-3.210", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = winterSemester, LectureLetter = "BB", Teacher = "T.B.A" });

            generalCourses.Add(new Course { CourseLetters = "MANA", CourseNumber = 300, Credit = 3, ElectiveType = ElectiveType.GeneralElective, Title = "ENTRPSHP:LAUNCH'G YOUR BSNSS" });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-5.275", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1050, Semester = fallSemester, LectureLetter = "A", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-S1.430", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.NONE, StartTime = 705, EndTime = 870, Semester = winterSemester, LectureLetter = "B", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "EV-1.615", FirstDay = TimeBlock.day.F, SecondDay = TimeBlock.day.NONE, StartTime = 525, EndTime = 690, Semester = winterSemester, LectureLetter = "C", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.210", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = winterSemester, LectureLetter = "AA", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });

            generalCourses.Add(new Course { CourseLetters = "MARK", CourseNumber = 201, Credit = 3, ElectiveType = ElectiveType.GeneralElective, Title = "INTRODUCTION TO MARKETING" });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-3.270", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 1110, EndTime = 1260, Semester = summerOneSemester, LectureLetter = "CA", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.270", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 705, EndTime = 870, Semester = fallSemester, LectureLetter = "A", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-3.270", FirstDay = TimeBlock.day.F, SecondDay = TimeBlock.day.NONE, StartTime = 525, EndTime = 690, Semester = fallSemester, LectureLetter = "C", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.270", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1050, Semester = fallSemester, LectureLetter = "B", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "HB-130", FirstDay = TimeBlock.day.J, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1050, Semester = fallSemester, LectureLetter = "01", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.270", FirstDay = TimeBlock.day.J, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = fallSemester, LectureLetter = "AA", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "HB-130", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = fallSemester, LectureLetter = "51", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-3.270", FirstDay = TimeBlock.day.F, SecondDay = TimeBlock.day.NONE, StartTime = 705, EndTime = 855, Semester = winterSemester, LectureLetter = "F", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.270", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.NONE, StartTime = 705, EndTime = 855, Semester = winterSemester, LectureLetter = "E", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.270", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 525, EndTime = 690, Semester = winterSemester, LectureLetter = "D", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "HB-130", FirstDay = TimeBlock.day.J, SecondDay = TimeBlock.day.NONE, StartTime = 885, EndTime = 1035, Semester = winterSemester, LectureLetter = "02", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "MB-2.210", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 1065, EndTime = 1215, Semester = winterSemester, LectureLetter = "BB", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });

            generalCourses.Add(new Course { CourseLetters = "URBS", CourseNumber = 230, Credit = 3, ElectiveType = ElectiveType.GeneralElective, Title = " URBAN DEVELOPMENT" });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "N/A", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.W, StartTime = 975, EndTime = 1050, Semester = fallSemester, LectureLetter = "A", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });
            generalLectures.Add(new Lecture { Course = generalCourses.LastOrDefault(), ClassRoomNumber = "N/A", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 705, EndTime = 780, Semester = winterSemester, LectureLetter = "A", Teacher = "T.B.A" });
            context.Section.Add(new Section { Lecture = generalLectures.LastOrDefault() });



            context.Courses.AddRange(generalCourses);
            context.Lectures.AddRange(generalLectures);

            context.SaveChanges();


        }
        List<Course> AddScienceElectives (ApplicationDbContext context)
        {
            List<Course> scienceCourses = new List<Course>();
            List<Lecture> scienceLectures = new List<Lecture>();
            List<Tutorial> tutorialLectures = new List<Tutorial>();
            scienceCourses.Add(new Course { CourseLetters = "BIOL", CourseNumber = 206, Credit = 3, ElectiveType = ElectiveType.BasicScience, Title = "Elementary Genetics" });
            scienceLectures.Add(new Lecture { Course = scienceCourses.LastOrDefault(), ClassRoomNumber = "HC-155", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 855, EndTime = 960, Semester = winterSemester, LectureLetter = "01", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = scienceLectures.LastOrDefault() });

            scienceCourses.Add(new Course { CourseLetters = "BIOL", CourseNumber = 261, Credit = 3, ElectiveType = ElectiveType.BasicScience, Title = "MOLECULAR & GENERAL GENETICS" });
            scienceLectures.Add(new Lecture { Course = scienceCourses.LastOrDefault(), ClassRoomNumber = "	SP-S110", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.F, StartTime = 705, EndTime = 780, Semester = fallSemester, LectureLetter = "01", Teacher = "N/A" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.W, StartTime = 810, EndTime = 930, TutorialLetter = "03" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.125", FirstDay = TimeBlock.day.J, StartTime = 810, EndTime = 930, TutorialLetter = "05" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CC-425", FirstDay = TimeBlock.day.J, StartTime = 810, EndTime = 930, TutorialLetter = "07" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CC-312", FirstDay = TimeBlock.day.T, StartTime = 810, EndTime = 930, TutorialLetter = "01" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.W, StartTime = 930, EndTime = 1050, TutorialLetter = "04" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.J, StartTime = 930, EndTime = 1050, TutorialLetter = "06" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.J, StartTime = 930, EndTime = 1050, TutorialLetter = "08" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.T, StartTime = 930, EndTime = 1050, TutorialLetter = "02" });

            scienceLectures.Add(new Lecture { Course = scienceCourses.LastOrDefault(), ClassRoomNumber = "	SP-S110", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.F, StartTime = 705, EndTime = 780, Semester = winterSemester, LectureLetter = "02", Teacher = "N/A" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.W, StartTime = 810, EndTime = 930, TutorialLetter = "03" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.125", FirstDay = TimeBlock.day.J, StartTime = 810, EndTime = 930, TutorialLetter = "05" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CC-425", FirstDay = TimeBlock.day.J, StartTime = 810, EndTime = 930, TutorialLetter = "07" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CC-312", FirstDay = TimeBlock.day.T, StartTime = 810, EndTime = 930, TutorialLetter = "01" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.W, StartTime = 930, EndTime = 1050, TutorialLetter = "04" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.J, StartTime = 930, EndTime = 1050, TutorialLetter = "06" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.J, StartTime = 930, EndTime = 1050, TutorialLetter = "08" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "CJ-1.121", FirstDay = TimeBlock.day.T, StartTime = 930, EndTime = 1050, TutorialLetter = "02" });

            scienceCourses.Add(new Course { CourseLetters = "CHEM", CourseNumber = 217, Credit = 3, ElectiveType = ElectiveType.BasicScience, Title = "INTRO ANALYTICAL CHEMISTRY I" });
            scienceLectures.Add(new Lecture { Course = scienceCourses.LastOrDefault(), ClassRoomNumber = "CC-321", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.F, StartTime = 615, EndTime = 690, Semester = fallSemester, LectureLetter = "01", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = scienceLectures.LastOrDefault() });
            scienceLectures.Add(new Lecture { Course = scienceCourses.LastOrDefault(), ClassRoomNumber = "CC-321", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.F, StartTime = 1080, EndTime = 1230, Semester = fallSemester, LectureLetter = "51", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = scienceLectures.LastOrDefault() });

            scienceCourses.Add(new Course { CourseLetters = "CIVI", CourseNumber = 231, Credit = 3, ElectiveType = ElectiveType.BasicScience, Title = "GEOLOGY FOR CIVIL ENGINEERS" });
            scienceLectures.Add(new Lecture { Course = scienceCourses.LastOrDefault(), ClassRoomNumber = "H-937", FirstDay = TimeBlock.day.M,  StartTime = 1065, EndTime = 1215, Semester = fallSemester, LectureLetter = "LL", Teacher = "N/A" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "H-605", FirstDay = TimeBlock.day.M, StartTime = 1230, EndTime = 1290, TutorialLetter = "LA" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "H-401", FirstDay = TimeBlock.day.T, StartTime = 1110, EndTime = 1170, TutorialLetter = "LB" });
            tutorialLectures.Add(new Tutorial { Lecture = scienceLectures.LastOrDefault(), ClassRoomNumber = "H-539", FirstDay = TimeBlock.day.W, StartTime = 1170, EndTime = 1230, TutorialLetter = "LC" });
            List<Section> scienceSection = new List<Section>();
            foreach (Tutorial tutorial in tutorialLectures)
            {
                Section section = new Section { Tutorial = tutorial, Lecture = tutorial.Lecture };
                CheckIfSectionHasSameTimes(section);
                sections.Add(section);
                context.Section.Add(sections.LastOrDefault());
            }
            
            context.Courses.AddRange(scienceCourses);
            context.Lectures.AddRange(scienceLectures);
            context.Tutorials.AddRange(tutorialLectures);
            context.Section.AddRange(scienceSection);
            context.Courses.Where(n => n.CourseLetters == "ENGR" && n.CourseNumber == 242).FirstOrDefault().ElectiveType = ElectiveType.BasicScience;
            context.Courses.Where(n => n.CourseLetters == "ENGR" && n.CourseNumber == 243).FirstOrDefault().ElectiveType = ElectiveType.BasicScience;
            context.Courses.Where(n => n.CourseLetters == "ENGR" && n.CourseNumber == 251).FirstOrDefault().ElectiveType = ElectiveType.BasicScience;
            context.Courses.Where(n => n.CourseLetters == "ENGR" && n.CourseNumber == 361).FirstOrDefault().ElectiveType = ElectiveType.BasicScience;
            context.Courses.Where(n => n.CourseLetters == "MECH" && n.CourseNumber == 221).FirstOrDefault().ElectiveType = ElectiveType.BasicScience;

            context.SaveChanges();
            return courses;
        }

        void AddMathElectives(ApplicationDbContext context)
        {
            List<Course> mathCourses = new List<Course>();
            List<Lecture> mathLectures = new List<Lecture>();
            List<Tutorial> tutorialLectures = new List<Tutorial>();

            mathCourses.Add(new Course { CourseLetters = "MAST", CourseNumber = 218, Credit = 3, ElectiveType = ElectiveType.MathElective, Title = "Multivariable Calculus I" });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "H-521", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 615, EndTime = 690, Semester = fallSemester, LectureLetter = "A", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "MB-S1.401", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.F, StartTime = 705, EndTime = 780, Semester = fallSemester, LectureLetter = "B", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "H-535", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.NONE, StartTime = 1080, EndTime = 1215, Semester = winterSemester, LectureLetter = "AA", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });

            mathCourses.Add(new Course { CourseLetters = "MAST", CourseNumber = 219, Credit = 3, ElectiveType = ElectiveType.MathElective, Title = "Multivariable Calculus II" });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "MB-2.210", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.W, StartTime = 885, EndTime = 1080, Semester = fallSemester, LectureLetter = "A", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "MB-2.210", FirstDay = TimeBlock.day.J, SecondDay = TimeBlock.day.NONE, StartTime = 1080, EndTime = 1215, Semester = winterSemester, LectureLetter = "AA", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });

            mathCourses.Add(new Course { CourseLetters = "MAST", CourseNumber = 224, Credit = 3, ElectiveType = ElectiveType.MathElective, Title = "Introduction to Optimization" });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "H-423", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 885, EndTime = 1080, Semester = winterSemester, LectureLetter = "A", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });

            mathCourses.Add(new Course { CourseLetters = "MAST", CourseNumber = 234, Credit = 3, ElectiveType = ElectiveType.MathElective, Title = "Linear Algebra and Applications I" });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "LB-915", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.F, StartTime = 615, EndTime = 690, Semester = fallSemester, LectureLetter = "A", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "LB-915", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 615, EndTime = 690, Semester = winterSemester, LectureLetter = "B", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });

            mathCourses.Add(new Course { CourseLetters = "MAST", CourseNumber = 235, Credit = 3, ElectiveType = ElectiveType.MathElective, Title = "Linear Algebra and Applications II" });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "LB-915", FirstDay = TimeBlock.day.W, SecondDay = TimeBlock.day.F, StartTime = 615, EndTime = 690, Semester = winterSemester, LectureLetter = "A", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });

            mathCourses.Add(new Course { CourseLetters = "MAST", CourseNumber = 332, Credit = 3, ElectiveType = ElectiveType.MathElective, Title = "Techniques in Symbolic Computation" });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "H-401", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 705, EndTime = 780, Semester = winterSemester, LectureLetter = "A", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });

            mathCourses.Add(new Course { CourseLetters = "MAST", CourseNumber = 334, Credit = 3, ElectiveType = ElectiveType.MathElective, Title = "Numerical Analysis" });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "MB-3.210", FirstDay = TimeBlock.day.T, SecondDay = TimeBlock.day.J, StartTime = 975, EndTime = 1050, Semester = fallSemester, LectureLetter = "A", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });
            mathLectures.Add(new Lecture { Course = mathCourses.LastOrDefault(), ClassRoomNumber = "FG-B030", FirstDay = TimeBlock.day.M, SecondDay = TimeBlock.day.NONE, StartTime = 1080, EndTime = 1215, Semester = fallSemester, LectureLetter = "AA", Teacher = "N/A" });
            context.Section.Add(new Section { Lecture = mathLectures.LastOrDefault() });

            context.SaveChanges();
        }
        void AddPrerequisite( ApplicationDbContext context)
        {
            List<Course> courses = context.Courses.ToList();
            List<Prerequisite> prerequisites = new List<Prerequisite>();
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 287).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 248).FirstOrDefault()});
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 321).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 346).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 331).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 232).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 331).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 249).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 341).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 341).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENCS" && m.CourseNumber == 282).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 342).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 341).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 343).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 341).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 343).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 342).FirstOrDefault(), Concurrently = true });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 344).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 343).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 345).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 341).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 345).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 343).FirstOrDefault(), Concurrently = true });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 357).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 342).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 384).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENCS" && m.CourseNumber == 282).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 384).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 341).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 385).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 213).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 385).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 233).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 387).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 353).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 387).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 341).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 387).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 287).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 390).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 344).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 390).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 357).FirstOrDefault(), Concurrently = true });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 422).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 346).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 423).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 346).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 487).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 387).FirstOrDefault(), Concurrently = true });

            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 490).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 342).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 490).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 343).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 490).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 344).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 490).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 390).FirstOrDefault() });

            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 242).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 213).FirstOrDefault(), Concurrently = true });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 243).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 213).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 243).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 242).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 371).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 213).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 371).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 233).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 391).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 213).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 391).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 233).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 391).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 248).FirstOrDefault() });

            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 228).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 248).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 249).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 248).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 335).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 232).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 335).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 249).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 339).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 232).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 345).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault(), Concurrently = true });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 346).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 228).FirstOrDefault(), OrPrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 228).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 346).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault()});
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 348).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 249).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 232).FirstOrDefault(), Concurrently = true });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 249).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 353).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 354).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENCS" && m.CourseNumber == 282).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 361).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 232).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 361).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 249).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 371).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 376).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 371).FirstOrDefault(), Concurrently = true });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 426).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 346).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 428).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 346).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 442).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 335).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 442).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 445).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 346).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 465).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 335).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 465).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 472).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 473).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 474).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 476).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 376).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 476).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 361).FirstOrDefault(), OrPrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 391).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 477).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 371).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 477).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 361).FirstOrDefault(), OrPrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 391).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 478).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 352).FirstOrDefault() });
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 479).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "COMP" && m.CourseNumber == 233).FirstOrDefault(), OrPrerequisiteCourse = courses.Where(m => m.CourseLetters == "ENGR" && m.CourseNumber == 371).FirstOrDefault() });

            foreach (Course course in courses)
            {
                List<Prerequisite> prerequisiteToAddToCourse = prerequisites.Where(n => n.Course == course).ToList();
                if (prerequisiteToAddToCourse.Count() > 0)
                    course.Prerequisites = prerequisiteToAddToCourse;
                context.Entry(course).State = EntityState.Modified;
            }
            context.Prerequisite.AddRange(prerequisites);
            
            context.SaveChanges();

        }
        public struct time
        {
            public double startTime;
            public double endTime;
            public Schedulator.Models.TimeBlock.day firstDay;
            public Schedulator.Models.TimeBlock.day secondDay;
        }
        public time ParseTime(string cell)
        {
            time timeToReturn = new time();
            MatchCollection mc = Regex.Matches(cell, @"\d{2}:\d{2}");
            IEnumerator enumMatch = mc.GetEnumerator();
            enumMatch.MoveNext();
            Match mach = (Match)enumMatch.Current;
            string time = mach.Value;
            string timeHours = time[0].ToString() + time[1].ToString();
            string timeMinutes = time[3].ToString() + time[4].ToString();
            timeToReturn.startTime = Convert.ToDouble(timeHours) * 60 + Convert.ToDouble(timeMinutes);
            enumMatch.MoveNext();
            mach = (Match)enumMatch.Current;
            time = mach.Value;
            timeHours = time[0].ToString() + time[1].ToString();
            timeMinutes = time[3].ToString() + time[4].ToString();
            timeToReturn.endTime = Convert.ToDouble(timeHours) * 60 + Convert.ToDouble(timeMinutes);
            string days = Regex.Match(cell, @"[MTWJFSD-]{7}").Value;
            bool onSecondDay = false;
            timeToReturn.firstDay = TimeBlock.day.NONE;
            timeToReturn.secondDay = TimeBlock.day.NONE;
            if (days[0] == 'M')
            {
                onSecondDay = true;
                timeToReturn.firstDay = Schedulator.Models.TimeBlock.day.M;
            }
            if (days[1] == 'T')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = Schedulator.Models.TimeBlock.day.T;
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = Schedulator.Models.TimeBlock.day.T;
                }
            }
            if (days[2] == 'W')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = Schedulator.Models.TimeBlock.day.W;
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = Schedulator.Models.TimeBlock.day.W;
                }
            }
            if (days[3] == 'J')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = Schedulator.Models.TimeBlock.day.J;
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = Schedulator.Models.TimeBlock.day.J;
                }
            }
            if (days[4] == 'F')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = Schedulator.Models.TimeBlock.day.F;
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = Schedulator.Models.TimeBlock.day.F;
                }
            }
            if (days[1] == 'S')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = Schedulator.Models.TimeBlock.day.S;
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = Schedulator.Models.TimeBlock.day.S;
                }
            }
            if (days[2] == 'D')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = Schedulator.Models.TimeBlock.day.D;
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = Schedulator.Models.TimeBlock.day.D;
                }
            }
            return timeToReturn;
        }
    }

}
