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

        public List<TranscriptYear> TranscriptYear { get; set; }

        public class TranscriptYear
        {
            public int year { get; set; }
            public List<TranscriptSemester> StudentYearEnrollments { get; set; }

            public class TranscriptSemester
            {
                public Semester Semester { get; set; }
                public List<Enrollment> StudentEnrollments { get; set; }
            }
        }


        public void StudentTranscript(List<Enrollment> studentEnrollments)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (studentEnrollments != null && studentEnrollments.Count != 0) 
            {
                int studentStartYear = studentEnrollments.FirstOrDefault().Schedule.Semester.SemesterStart.Year;
                Season startSeason = studentEnrollments.FirstOrDefault().Schedule.Semester.Season;

                HashSet<int> studentEnrollmentYearSpan = new HashSet<int>();
                foreach (Enrollment enrollment in studentEnrollments)
                {
                    studentEnrollmentYearSpan.Add(enrollment.Schedule.Semester.SemesterStart.Year);
                }
               

            }
        }

        public double GetSemesterGPA()
        {
            return 1.0;
        }

    }

    
    
}