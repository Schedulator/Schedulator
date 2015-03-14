using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class ScheduleGenerator
    {
        public Preference Preference { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
   
        public void GenerateSchedules(List<Course> courses, List<Enrollment> enrollments, Program program)
        {
            if (Preference.UseCourseSequence) // Generate from Program sequence passed
            {
                List<Course> coursesFromCourseSequence = new List<Course>();
               
                foreach ( CourseSequence courseSequence in program.courseSequences.OrderBy(p => p.CourseSequenceId))
                {
                    foreach (Enrollment enrollment in enrollments)
                    {
                        if (enrollment.Course == courseSequence.Course)
                            break;
                    }

                }
            }
        }
    }
}