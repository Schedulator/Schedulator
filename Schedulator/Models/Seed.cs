using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;

namespace Schedulator.Models
{
    public class Seed
    {
        public struct time
        {
            public double startTime;
            public double endTime;
            public string firstDay;
            public string secondDay;
        }
        public time ParseTime(string cell)
        {
            time timeToReturn = new time();
            MatchCollection mc = Regex.Matches(cell, @"\d{2}:\d{2}");
            IEnumerator enumMatch = mc.GetEnumerator();
            enumMatch.MoveNext();
            Match mach = (Match)enumMatch.Current;
            string time = mach.Value;
            time = time[0].ToString() + time[1].ToString() + "." + time[3].ToString() + time[4].ToString();
            timeToReturn.startTime = Convert.ToDouble(time);
            enumMatch.MoveNext();
            mach = (Match)enumMatch.Current;
            time = mach.Value;
            time = time[0].ToString() + time[1].ToString() + "." + time[3].ToString() + time[4].ToString();
            timeToReturn.endTime = Convert.ToDouble(time);
            string days = Regex.Match(cell, @"[MTWJFSD-]{7}").Value;
            bool onSecondDay = false;
            if (days[0] == 'M')
            {
                onSecondDay = true;
                timeToReturn.firstDay = "M";
            }
            if (days[1] == 'T')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = "T";
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = "T";
                }
            }
            if (days[1] == 'W')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = "W";
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = "W";
                }
            }
            if (days[3] == 'J')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = "J";
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = "J";
                }
            }
            if (days[4] == 'F')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = "F";
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = "F";
                }
            }
            if (days[1] == 'S')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = "S";
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = "S";
                }
            }
            if (days[2] == 'D')
            {
                if (onSecondDay)
                    timeToReturn.secondDay = "D";
                else
                {
                    onSecondDay = true;
                    timeToReturn.firstDay = "D";
                }
            }
            return timeToReturn;
        }
        public void SeedDatabase()
        {
            
                var fallSemester = new Semester { Season = Season.Fall, SemesterStart = new DateTime(2014, 9, 1), SemesterEnd = new DateTime(2014, 12, 18) };
                var winterSemester = new Semester { Season = Season.Winter, SemesterStart = new DateTime(2015, 1, 7), SemesterEnd = new DateTime(2015, 5, 2) };
                var summerOneSemester = new Semester { Season = Season.Summer1, SemesterStart = new DateTime(2015, 5, 4), SemesterEnd = new DateTime(2015, 6, 23) };
                var summerTwoSemester = new Semester { Season = Season.Summer2, SemesterStart = new DateTime(2015, 6, 25), SemesterEnd = new DateTime(2015, 7, 19) };

                var courses = new List<Course>();
                var lectures = new List<Lecture>();
                var tutorials = new List<Tutorial>();
                var labs = new List<Lab>();
                var prerequisites = new List<Prerequisite>();

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
                                        }

                                    }
                                }
                            }


                        }
                        else
                            count++;
                    }

                }
                foreach (Course course in courses)
                    Console.WriteLine(course.CourseLetters + course.CourseNumber);
            }

    }
}