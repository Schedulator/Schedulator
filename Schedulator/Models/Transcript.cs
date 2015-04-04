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

        //public List<TranscriptYear> TrasncriptYears { get; set; }

        //public class TranscriptYear
        //{
        //    public int Year { get; set; }
        //    public List<TranscriptSemester> TranscriptSemesters { get; set; }

        //    public class TranscriptSemester
        //    {
        //        public Semester Semester { get; set; }
        //        public List<Enrollment> Enrollments { get; set; }
        //    }
        //}


        //public void StudentTranscript(List<Enrollment> studentEnrollments)
        //{
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    if (studentEnrollments != null && studentEnrollments.Count != 0) 
        //    {
        //        int studentStartYear = studentEnrollments.FirstOrDefault().Schedule.Semester.SemesterStart.Year;
        //        Season startSeason = studentEnrollments.FirstOrDefault().Schedule.Semester.Season;
                

        //        HashSet<int> studentEnrollmentYearSpan = new HashSet<int>();
        //        foreach (Enrollment enrollment in studentEnrollments)
        //            studentEnrollmentYearSpan.Add(enrollment.Schedule.Semester.SemesterStart.Year);
               
        //        foreach (int year in studentEnrollmentYearSpan)
        //        {
        //            TrasncriptYears.Add(new TranscriptYear { Year = year });
        //            foreach (Enrollment enrollment in studentEnrollments.Where(n => n.Schedule.Semester.SemesterStart.Year == year))
        //            {
        //                for 
        //            }


        //            //switch (startSeason)
        //            //{
        //            //    case Season.Fall:
        //            //        {
        //            //            createTranscript(startSeason, studentStartYear);
        //            //        }
        //            //        break;
        //            //    case Season.Winter:
        //            //        {

        //            //        }
        //            //        break;
        //            //    case Season.Summer1:
        //            //        {
        //            //            throw new NullReferenceException("Trying to start from a summer semester");
        //            //        }
        //            //    case Season.Summer2:
        //            //        {
        //            //            throw new NullReferenceException("Trying to start from a summer semester");
        //            //        }
        //            //}
        //        }
        //    }
        //}

        //private void createTranscript(Season startSeason, int studentStartYear)
        //{
            
        //}

        //public double GetSemesterGPA()
        //{
        //    return 1.0;
        //}

    }

    
    
}