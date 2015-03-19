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

            
            Console.WriteLine(currentDirectoryUrl);

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


            var fallSemester = new Semester { Season = Season.Fall, SemesterStart = new DateTime(2014, 9, 1), SemesterEnd = new DateTime(2014, 12, 18) };
            var winterSemester = new Semester { Season = Season.Winter, SemesterStart = new DateTime(2015, 1, 7), SemesterEnd = new DateTime(2015, 5, 2) };
            var summerOneSemester = new Semester { Season = Season.Summer1, SemesterStart = new DateTime(2015, 5, 4), SemesterEnd = new DateTime(2015, 6, 23) };
            var summerTwoSemester = new Semester { Season = Season.Summer2, SemesterStart = new DateTime(2015, 6, 25), SemesterEnd = new DateTime(2015, 7, 19) };

            context.Semesters.AddOrUpdate(fallSemester);
            context.Semesters.AddOrUpdate(winterSemester);
            context.Semesters.AddOrUpdate(summerOneSemester);
            context.Semesters.AddOrUpdate(summerTwoSemester);

            var courses = new List<Course>();
            var lectures = new List<Lecture>();
            var tutorials = new List<Tutorial>();
            var labs = new List<Lab>();
            var prerequisites = new List<Prerequisite>();
            var sections = new List<Section>();

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
                        courses.Add(new Course { CourseLetters = Regex.Match(currentCell, @"[A-Z]{4}").ToString(), CourseNumber = Convert.ToInt32(Regex.Match(currentCell, @"\d{3}").ToString()), Title = row[count].Cells[1].Text });

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
                                            sections.Add(new Section { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault() });
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
                                            sections.Add(new Section { Lecture = lectures.LastOrDefault(), Tutorial = tutorials.LastOrDefault(), Lab = labs.LastOrDefault() });
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
                                        sections.Add(new Section { Lecture = lectures.LastOrDefault(), Lab = labs.LastOrDefault() });
                                      //  lectures.LastOrDefault().Sections.Add(sections.LastOrDefault());
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

            Schedule schedule = new Schedule { ApplicationUser = context.Users.Where(u => u.FirstName == "Harley").FirstOrDefault(), Semester = fallSemester, IsRegisteredSchedule = true };

            List<Enrollment> enrollments = new List<Enrollment>();

            enrollments.Add(new Enrollment { Schedule = schedule, Section = sections.Where(t => t.Tutorial.TutorialLetter == "QB" && t.Lecture.Course.CourseLetters == "COMP" && t.Lecture.Course.CourseNumber == 232).FirstOrDefault(), Grade = "B-" });
            enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            enrollments.Add(new Enrollment { Schedule = schedule, Section = sections.Where(t => t.Tutorial.TutorialLetter == "AE" && t.Lecture.Course.CourseLetters == "COMP" && t.Lecture.Course.CourseNumber == 248).FirstOrDefault(), Grade = "A" });
            enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            enrollments.Add(new Enrollment { Schedule = schedule, Section = sections.Where(t => t.Lecture.Course.CourseLetters == "ENGR" && t.Lecture.Course.CourseNumber == 201 && t.Tutorial.TutorialLetter == "GA").FirstOrDefault(), Grade = "B+" });
            enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            enrollments.Add(new Enrollment { Schedule = schedule, Section = sections.Where(t => t.Lecture.Course.CourseLetters == "ENGR" && t.Lecture.Course.CourseNumber == 213 && t.Tutorial.TutorialLetter == "PA").FirstOrDefault(), Grade = "B+" });
            enrollments.LastOrDefault().Course = enrollments.LastOrDefault().Section.Lecture.Course;
            context.Schedule.AddOrUpdate(schedule);

            enrollments.ForEach(p => context.Enrollment.AddOrUpdate(p));

            AddPrerequisite(courses).ForEach(p => context.Prerequisite.Add(p));

           // List<Program> programs = AddProgramAndCourseSequence(@"C:\Users\Harley\Desktop\Schedulator\Schedulator\Programs.xlsx", context.Courses.ToList());
          //  programs.ForEach(p => context.Program.Add(p));
           // programs.ForEach(p => context.CourseSequence.AddRange(p.courseSequences));
            context.SaveChanges();
        }
        List<Program> AddProgramAndCourseSequence (string url, List<Course> courses)
        {
            List<Program> programs = new List<Program>();
            List<CourseSequence> courseSequences = new List<CourseSequence>();
            foreach (var worksheet in Workbook.Worksheets(url))
            {
                int count = 1;
                var rows = worksheet.Rows;
                programs.Add(new Program { ProgramName = rows[0].Cells[0].Text, ProgramOption = rows[0].Cells[1].Text, CreditsRequirement = Convert.ToInt32(rows[0].Cells[2].Text) });
                Season season = 0;
                int year = 0;
                while (count < rows.Count())
                {
                    string currentCellText = rows[count].Cells[0].Text;

                    if (Regex.IsMatch(currentCellText, @"Year\s\d{1}\s(Winter|Fall|Summer)"))
                    {
                        year = Convert.ToInt32(Regex.Match(currentCellText, @"\d{1}").ToString());
                        string temp = Regex.Match(currentCellText, @"(Fall|Winter|Summer)").ToString();
                        season = (Season)Enum.Parse(typeof(Season), Regex.Match(currentCellText, @"(Fall|Winter|Summer)").ToString());
                    }
                    else if ( Regex.IsMatch(currentCellText, @"[A-Z]{4}\s\d{3} or"))
                    {
                        
                        List<CourseSequence> otherCourseOptions = new List<CourseSequence>();
                        otherCourseOptions.Add(new CourseSequence { Program = programs.LastOrDefault(), OtherOptions = new List<CourseSequence>(), Year = year, Season = season });
                        ElectiveType electiveType = ElectiveType.None;
                       // if (currentCellText.Contains("Elective"))
                           // electiveType = ElectiveType.TechnicalElective;
                        currentCellText = rows[count].Cells[0].Text;
                        string mergeRows = "";
                        while (Regex.IsMatch(currentCellText,@"([A-Z]{4}\s\d{3} or|Elective)"))
                        {
                            mergeRows += Regex.Match(currentCellText, @"([A-Z]{4}\s\d{3} or|Elective)").ToString();
                            currentCellText = rows[++count].Cells[0].Text;
                        }

                        foreach (Match match in Regex.Matches(mergeRows, @"([A-Z]{4}\s\d{3}|Elective)"))
                        {
                            CourseSequence temp;
                            if ( match.ToString() != "Elective")
                                 temp = new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = electiveType, Year = year, Season = season, Course = courses.Where(p => p.CourseLetters == Regex.Match(match.ToString(), @"[A-Z]{4}").ToString() && p.CourseNumber == Convert.ToInt32(Regex.Match(match.ToString(), @"\d{3}").ToString())).FirstOrDefault() };
                            else
                                temp = new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = ElectiveType.TechnicalElective, Year = year, Season = season};
                            otherCourseOptions.Add(temp);
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
                        courseSequences.Add(new CourseSequence { Program = programs.LastOrDefault(), ElectiveType = Models.ElectiveType.TechnicalElective, Year = year, Season = season });
                    count++;
                }
                programs.LastOrDefault().courseSequences = courseSequences;
            }
            return programs;
        }
        List<Prerequisite> AddPrerequisite(List<Course> courses)
        {
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
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 384).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 337).FirstOrDefault() });
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
            prerequisites.Add(new Prerequisite { Course = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 490).FirstOrDefault(), PrerequisiteCourse = courses.Where(m => m.CourseLetters == "SOEN" && m.CourseNumber == 337).FirstOrDefault() });
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

            return prerequisites;
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
