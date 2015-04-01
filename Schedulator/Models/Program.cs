using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Program
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string ProgramOption { get; set; }
        public int CreditsRequirement { get; set; }
        public string ProgramSemester { get; set; }

        public virtual ICollection<CourseSequence> CourseSequences { get; set; }
        public virtual ICollection<Course> TechnicalElectiveCourses { get; set; }
        
        public class RecommendCourseView
        {
            public Program Program { get; set; }
            public List<CourseSequence> RecommendFallCourseSequence {get; set;}
            public List<CourseSequence> RecommendWinterCourseSequence {get; set;}
            public List<CourseSequence> RecommendSummerCourseSequence {get; set;}

            public Schedule FallSchedule { get; set; }
            public Schedule WinterSchedule { get; set; }
            public List<Schedule> SummerSchedules { get; set; }
        }
        public RecommendCourseView RecommendedCourseForStudent(List<Enrollment> studentEnrollments, Schedule fallSchedule, Schedule winterSchedule, List<Schedule> summerSchedules)
        {
            RecommendCourseView recommendCourseView = new RecommendCourseView { Program = this, RecommendFallCourseSequence = new List<CourseSequence>(), RecommendWinterCourseSequence = new List<CourseSequence>(), RecommendSummerCourseSequence = new List<CourseSequence>() };
            List<Enrollment> enrollmentCopy = studentEnrollments.ToList();

            if (fallSchedule != null)
                recommendCourseView.FallSchedule = fallSchedule;
            if (winterSchedule != null)
                recommendCourseView.WinterSchedule = winterSchedule;
            if (summerSchedules.Count() != 0)
                recommendCourseView.SummerSchedules = summerSchedules;
                        
            foreach (CourseSequence courseSequence in CourseSequences.OrderBy(n=> n.Year))
            {
                bool noEnrollment = true;
                foreach (Enrollment enrollment in studentEnrollments.ToList())
                {
                    if (courseSequence.Course == enrollment.Course)
                    {
                        noEnrollment = false;
                        studentEnrollments.Remove(enrollment);
                        break;
                    }
                    else if ((courseSequence.ElectiveType == ElectiveType.GeneralElective && enrollment.Course.ElectiveType == ElectiveType.GeneralElective)
                        || (courseSequence.ElectiveType == ElectiveType.TechnicalElective && enrollment.Course.ElectiveType == ElectiveType.TechnicalElective)
                        || (courseSequence.ElectiveType == ElectiveType.BasicScience && enrollment.Course.ElectiveType == ElectiveType.BasicScience))
                    {
                        noEnrollment = false;
                        studentEnrollments.Remove(enrollment);
                        break;
                        
                    }
                }
                if (noEnrollment)
                {
                    if (((fallSchedule == null && recommendCourseView.RecommendFallCourseSequence.Count() < 5 ) || (fallSchedule != null && fallSchedule.Enrollments.Count < 5 && recommendCourseView.RecommendFallCourseSequence.Count() < 5 -fallSchedule.Enrollments.Count)))
                        recommendCourseView.RecommendFallCourseSequence.Add(courseSequence);
                    else if (((winterSchedule == null && recommendCourseView.RecommendWinterCourseSequence.Count() < 5) || (winterSchedule != null && winterSchedule.Enrollments.Count < 5 && recommendCourseView.RecommendWinterCourseSequence.Count() < 5 - winterSchedule.Enrollments.Count)))
                        recommendCourseView.RecommendWinterCourseSequence.Add(courseSequence);
                    else if ((summerSchedules.Count() == 0 && recommendCourseView.RecommendSummerCourseSequence.Count < 5))
                        recommendCourseView.RecommendSummerCourseSequence.Add(courseSequence);
                    else if ( summerSchedules.Count() != 0)
                    {   
                        int numberOfEnrollments = 0;
                        if (summerSchedules.Count > 1)
                            numberOfEnrollments += summerSchedules.FirstOrDefault().Enrollments.Count + summerSchedules.LastOrDefault().Enrollments.Count;
                        else
                            numberOfEnrollments = summerSchedules.FirstOrDefault().Enrollments.Count();
                        if (recommendCourseView.RecommendSummerCourseSequence.Count < 5 - numberOfEnrollments)
                            recommendCourseView.RecommendSummerCourseSequence.Add(courseSequence); 
                    }


                }
                if (recommendCourseView.RecommendSummerCourseSequence.Count == 5)
                    break;
            }
         //   foreach (CourseSequence courseSequence in recommendedCourseList.ToList())
           // {
        //        if (courseSequence.Course != null && courseSequence.Course.MissingPrequisite(enrollmentCopy).Count > 0)
      //              recommendedCourseList.Remove(courseSequence);
      //      }

            return recommendCourseView;
        }
    }
}