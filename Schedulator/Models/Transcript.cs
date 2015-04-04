using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    // A transcript contains a list of years. A year contains a list of semesters. A semester contains a list of enrollments.
    public class Transcript
    {

        public List<TranscriptYear> TrasncriptYears { get; set; }

        public class TranscriptYear
        {
            public int Year { get; set; }
            public List<TranscriptSemester> TranscriptSemesters { get; set; }

            public class TranscriptSemester
            {
                public Season Season { get; set; }
                public List<Enrollment> Enrollments { get; set; }
            }
        }


        public void StudentTranscript(List<Enrollment> studentEnrollments)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (studentEnrollments != null && studentEnrollments.Count != 0) 
            {
                int studentStartYear = studentEnrollments.FirstOrDefault().Schedule.Semester.SemesterStart.Year;
                Season startSeason = studentEnrollments.FirstOrDefault().Schedule.Semester.Season;
                List<Enrollment> studentEnrollmentsRemove = studentEnrollments;

                HashSet<int> studentEnrollmentYearSpan = new HashSet<int>();
                foreach (Enrollment enrollment in studentEnrollments)
                    studentEnrollmentYearSpan.Add(enrollment.Schedule.Semester.SemesterStart.Year);
               
                foreach (int year in studentEnrollmentYearSpan)
                {
                    TranscriptYear newTranscriptYear = new TranscriptYear();
                    TranscriptYear.TranscriptSemester newSemester;

                    while (studentEnrollmentsRemove.Count != 0 && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year) 
                    { 
                        switch (studentEnrollments.FirstOrDefault().Schedule.Semester.Season)
                        {
                            case Season.Winter:
                                    {
                                        newSemester = new TranscriptYear.TranscriptSemester();
                                        newSemester.Season = Season.Winter;
                                        while (studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season.Winter && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
                                        {
                                            newSemester.Enrollments.Add(studentEnrollmentsRemove.FirstOrDefault());
                                            studentEnrollmentsRemove.RemoveAt(0);
                                        }
                                        newTranscriptYear.TranscriptSemesters.Add(newSemester); 
                                    }
                                    break;
                            case Season.Fall:
                                    {
                                        newSemester = new TranscriptYear.TranscriptSemester();
                                        newSemester.Season = Season.Fall;
                                        while (studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season.Fall && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
                                        {
                                            newSemester.Enrollments.Add(studentEnrollmentsRemove.FirstOrDefault());
                                            studentEnrollmentsRemove.RemoveAt(0);
                                        }
                                        newTranscriptYear.TranscriptSemesters.Add(newSemester); 
                                    }
                                    break;
                            case Season.Summer1:
                                    {
                                        newSemester = new TranscriptYear.TranscriptSemester();
                                        newSemester.Season = Season.Summer1;
                                        while (studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season.Summer1 && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
                                        {
                                            newSemester.Enrollments.Add(studentEnrollmentsRemove.FirstOrDefault());
                                            studentEnrollmentsRemove.RemoveAt(0);
                                        }
                                        newTranscriptYear.TranscriptSemesters.Add(newSemester); 
                                    }
                                    break;
                            case Season.Summer2:
                                    {
                                        newSemester = new TranscriptYear.TranscriptSemester();
                                        newSemester.Season = Season.Summer2;
                                        while (studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season.Summer2 && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
                                        {
                                            newSemester.Enrollments.Add(studentEnrollmentsRemove.FirstOrDefault());
                                            studentEnrollmentsRemove.RemoveAt(0);
                                        }
                                        newTranscriptYear.TranscriptSemesters.Add(newSemester); 
                                    }
                                    break;
                        }
                    }


                        
                    

                    //switch (startSeason)
                    //{
                    //    case Season.Fall:
                    //        {
                    //            newSemester = new TranscriptYear.TranscriptSemester { Season = startSeason };
                    //        }
                    //        break;
                    //    case Season.Winter:
                    //        {

                    //        }
                    //        break;
                    //    case Season.Summer1:
                    //        {
                    //            throw new NullReferenceException("Trying to start from a summer semester");
                    //        }
                    //    case Season.Summer2:
                    //        {
                    //            throw new NullReferenceException("Trying to start from a summer semester");
                    //        }
                    //}
                    
                }
            }
        }

        private void addSemesterEnrollments(List<Enrollment> studentEnrollmentsRemove, Season Season, int year)
        {
            while (studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
            {
                
            }
        }

        public double GetSemesterGPA()
        {
            return 1.0;
        }

    }

    
    
}