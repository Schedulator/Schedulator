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

        public List<TranscriptYear> TranscriptYears { get; set; }
        public double totalCredits { get; set; }
        public double numeratorTotal { get; set; }
        public double CumulativeGPA{ get; set; }      

        public void GetTotalGPA() 
        {
            calculateNumeratorTotal();
            calculateTotalCredits();

            if (totalCredits != 0.0)
                CumulativeGPA = numeratorTotal / totalCredits;
            else
                CumulativeGPA = -1;
        }

        private void calculateTotalCredits()
        {
            totalCredits = 0.0;
            foreach (TranscriptYear year in TranscriptYears.Where(year => year != null))
            {
                totalCredits += year.totalYearCredits;
            }
        }

        private void calculateNumeratorTotal()
        {
            numeratorTotal = 0.0;
            foreach (TranscriptYear year in TranscriptYears.Where(year => year != null))
            {
                numeratorTotal += year.numeratorYearTotal;
            }
        }
        public class TranscriptYear
        {
            public int Year { get; set; }
            public List<TranscriptSemester> TranscriptSemesters { get; set; }
            public double totalYearCredits { get; set; }
            public double numeratorYearTotal { get; set; }
            public double YearGPA { get; set; }


            public void GetYearGPA()
            {
                calculateNumeratorYearTotal();
                calculateTotalYearCredits();

                if (totalYearCredits != 0.0)
                    YearGPA = numeratorYearTotal / totalYearCredits;
                else
                    YearGPA = -1;
            }

            public void calculateTotalYearCredits()
            {
                foreach (TranscriptSemester semester in TranscriptSemesters.Where(semester => semester != null))
                {
                    totalYearCredits += semester.totalSemesterCredits;
                }
            }

            public void calculateNumeratorYearTotal()
            {
                foreach (TranscriptSemester semester in TranscriptSemesters.Where(semester => semester != null))
                {
                    numeratorYearTotal += semester.numeratorSemesterTotal;
                }
            }

            public class TranscriptSemester
            {
                public Season Season { get; set; }
                public List<Enrollment> Enrollments { get; set; }
                public double totalSemesterCredits { get; set; }
                public double numeratorSemesterTotal { get; set; }
                public double SemesterGPA { get; set; }

                public void GetSemesterGPA()
                {
                    if (Enrollments != null)
                    {
                        calculateTotalSemesterCredits();
                        calculateNumeratorSemesterTotal();

                        if (totalSemesterCredits != 0.0)
                            SemesterGPA = numeratorSemesterTotal / totalSemesterCredits;
                        else 
                            SemesterGPA = -1;
                    }
                }

                public void calculateTotalSemesterCredits()
                {
                    totalSemesterCredits = 0.0;
                    foreach (Enrollment enrollment in Enrollments)
                    {
                        if (enrollment.Grade != null)
                        {
                            totalSemesterCredits += enrollment.Course.Credit;
                        }
                    }
                }

                public void calculateNumeratorSemesterTotal()
                {
                    numeratorSemesterTotal = 0.0;
                    foreach (Enrollment enrollment in Enrollments)
                    {
                        if (enrollment.Grade != null)
                        {
                            numeratorSemesterTotal += enrollment.getGPA() * enrollment.Course.Credit;
                        }
                    }
                }
            }
        }


        public void StudentTranscript(List<Enrollment> studentEnrollments)
        {

            // We cannot create a transcript if the student is not enrolled in any courses
            if (studentEnrollments != null && studentEnrollments.Count != 0) 
            {
                TranscriptYears = new List<TranscriptYear>();
                List<Enrollment> studentEnrollmentsRemove = studentEnrollments.ToList(); // This will serve as a checklist.

                // This will get a set of the year span of the student's enrollments (e.g. the student has enrolled courses in 2012, 2013, 2014, 2015)
                HashSet<int> studentEnrollmentYearSpan = new HashSet<int>();
                foreach (Enrollment enrollment in studentEnrollments)
                    studentEnrollmentYearSpan.Add(enrollment.Schedule.Semester.SemesterStart.Year);
               
                // For every year in which the student has been enrolled
                foreach (int year in studentEnrollmentYearSpan)
                {
                    TranscriptYear newTranscriptYear = new TranscriptYear { TranscriptSemesters = new List<TranscriptYear.TranscriptSemester>() };
                    TranscriptYear.TranscriptSemester newSemester; // Create a dummy semester
                    newTranscriptYear.Year = year;

                    // While there are enrollments remaining to check, and while these enrollments are in the desired year
                    while (studentEnrollmentsRemove.Count != 0 && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year) 
                    { 
                        // Check the first enrollment's season type. This means that the following couple of enrollments should also match this season type. Therefore
                        switch (studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season)
                        {
                            case Season.Winter:
                                    {
                                        newSemester = new TranscriptYear.TranscriptSemester { Season = Season.Winter, Enrollments = new List<Enrollment>() }; // Instantiate the dummy semester, set its season and instantiate its list of enrollments
                                        
                                        // While the first enrollment's type is still of the desired type and its year is the desired year
                                        while (studentEnrollmentsRemove.Count != 0 && studentEnrollmentsRemove != null/* CHECK FOR NULL*/ && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season.Winter && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
                                        {
                                            // Add the desired enrollment in the semester and remove the enrollment from the checklist.
                                            newSemester.Enrollments.Add(studentEnrollmentsRemove.FirstOrDefault());
                                            studentEnrollmentsRemove.RemoveAt(0);
                                        }
                                        newSemester.GetSemesterGPA(); // Calculate the semester's GPA
                                        newTranscriptYear.TranscriptSemesters.Add(newSemester); // Add the transcript semester to the transcript year
                                    }
                                    break;
                            case Season.Fall:
                                    {
                                        newSemester = new TranscriptYear.TranscriptSemester { Season = Season.Fall, Enrollments = new List<Enrollment>() }; // Instantiate the dummy semester, set its season and instantiate its list of enrollments

                                        // While the first enrollment's type is still of the desired type and its year is the desired year
                                        while (studentEnrollmentsRemove.Count != 0 && studentEnrollmentsRemove != null/* CHECK FOR NULL*/ && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season.Fall && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
                                        {
                                            // Add the desired enrollment in the semester and remove the enrollment from the checklist.
                                            newSemester.Enrollments.Add(studentEnrollmentsRemove.FirstOrDefault());
                                            studentEnrollmentsRemove.RemoveAt(0);
                                        }
                                        newSemester.GetSemesterGPA(); // Calculate the semester's GPA
                                        newTranscriptYear.TranscriptSemesters.Add(newSemester); // Add the transcript semester to the transcript year
                                    }
                                    break;
                            case Season.Summer1:
                                    {
                                        newSemester = new TranscriptYear.TranscriptSemester { Season = Season.Summer1, Enrollments = new List<Enrollment>() }; // Instantiate the dummy semester, set its season and instantiate its list of enrollments

                                        // While the first enrollment's type is still of the desired type and its year is the desired year
                                        while (studentEnrollmentsRemove.Count != 0 && studentEnrollmentsRemove != null/* CHECK FOR NULL*/ && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season.Summer1 && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
                                        {
                                            // Add the desired enrollment in the semester and remove the enrollment from the checklist.
                                            newSemester.Enrollments.Add(studentEnrollmentsRemove.FirstOrDefault());
                                            studentEnrollmentsRemove.RemoveAt(0);
                                        }
                                        newSemester.GetSemesterGPA(); // Calculate the semester's GPA
                                        newTranscriptYear.TranscriptSemesters.Add(newSemester); // Add the transcript semester to the transcript year
                                    }
                                    break;
                            case Season.Summer2:
                                    {
                                        newSemester = new TranscriptYear.TranscriptSemester { Season = Season.Summer2, Enrollments = new List<Enrollment>() }; // Instantiate the dummy semester, set its season and instantiate its list of enrollments

                                        // While the first enrollment's type is still of the desired type and its year is the desired year
                                        while (studentEnrollmentsRemove.Count != 0 && studentEnrollmentsRemove != null/* CHECK FOR NULL*/ && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.Season == Season.Summer2 && studentEnrollmentsRemove.FirstOrDefault().Schedule.Semester.SemesterStart.Year == year)
                                        {
                                            // Add the desired enrollment in the semester and remove the enrollment from the checklist.
                                            newSemester.Enrollments.Add(studentEnrollmentsRemove.FirstOrDefault());
                                            studentEnrollmentsRemove.RemoveAt(0);
                                        }
                                        newSemester.GetSemesterGPA(); // Calculate the semester's GPA
                                        newTranscriptYear.TranscriptSemesters.Add(newSemester); // Add the transcript semester to the transcript year
                                    }
                                    break;
                        }
                        newTranscriptYear.GetYearGPA();
                        // Add the completed year to the instance variable.
                        TranscriptYears.Add(newTranscriptYear);
                    }
                }
                this.GetTotalGPA();
            }
        }
    }
}