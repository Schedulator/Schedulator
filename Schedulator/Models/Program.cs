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
        
        public List<CourseSequence> RecommendedCourseForStudent (List<Enrollment> studentEnrollments)
        {
            List<CourseSequence> recommendedCourseList = new List<CourseSequence>();
            
            foreach (CourseSequence courseSequence in CourseSequences.OrderBy(n=> n.Year))
            {
                bool noEnrollment = true;
                foreach (Enrollment enrollment in studentEnrollments)
                {
                    if (courseSequence.Course == enrollment.Course)
                    {
                        noEnrollment = false;
                        break;
                    }
                }
                if (noEnrollment)
                    recommendedCourseList.Add(courseSequence);
                if (recommendedCourseList.Count == 10)
                    break;
            }
            foreach (CourseSequence courseSequence in recommendedCourseList.ToList())
            {
                if (courseSequence.Course != null && courseSequence.Course.MissingPrequisite(studentEnrollments).Count > 0)
                    recommendedCourseList.Remove(courseSequence);
            }

            return recommendedCourseList;
        }
    }
}