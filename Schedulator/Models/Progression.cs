using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Progression
    {
        public List<CourseSequence> CompletedCourse { get; set; }
        public List<CourseSequence> InProgressCourse { get; set; }
        public List<CourseSequence> IncompleteCourse { get; set; }

        public void StudentsProgression(List<Enrollment> studentsEnrollment, List<CourseSequence> courseSequences)
        {
            List<CourseSequence> courseSequencesRemoved = courseSequences.ToList();
            foreach (Enrollment enrollment in studentsEnrollment)
            {
                foreach(CourseSequence courseSequence in courseSequences)
                {
                    if (enrollment.Course == courseSequence.Course)
                    {
                        if (enrollment.Grade != null)
                            CompletedCourse.Add(courseSequence);
                        else
                            InProgressCourse.Add(courseSequence);
                        courseSequencesRemoved.Remove(courseSequence);
                    }
                    //foreach (CourseSequence option in courseSequence.OtherOptions)
                    //{
                    //    if (enrollment.Course == courseSequence.Course)
                    //    {
                    //        if (enrollment.Grade != null)
                    //            CompletedCourse.Add(courseSequence);
                    //        else
                    //            InProgressCourse.Add(courseSequence);
                    //        IncompleteCourse.Add(option);
                    //    }
                    //}
                }
                courseSequences = courseSequencesRemoved;
            }
            IncompleteCourse.AddRange(courseSequences);
        }

    }
}