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
                        studentEnrollments.Remove(enrollment);
                        noEnrollment = false;
                        break;
                    }
                }
                if (noEnrollment)
                    recommendedCourseList.Add(courseSequence);
            }

            return recommendedCourseList;
        }
    }
}