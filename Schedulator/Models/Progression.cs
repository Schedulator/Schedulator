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
            ApplicationDbContext db = new ApplicationDbContext();
            List<Course> BasicScienceCourses = db.Courses.Where(n => n.ElectiveType == ElectiveType.BasicScience).ToList();
            List<Course> GeneralCourses = db.Courses.Where(n => n.ElectiveType == ElectiveType.GeneralElective).ToList();
            List<CourseSequence> courseSequencesRemoved = courseSequences.ToList();
            foreach (Enrollment enrollment in studentsEnrollment)
            {
                foreach(CourseSequence courseSequence in courseSequences)
                {
                    if (courseSequence.OtherOptions.Count() > 0)
                    {
                        bool noneFound = true; ;
                        foreach (CourseSequence courseSequenceOtherOptions in courseSequence.OtherOptions)
                        {
                            if (enrollment.Course == courseSequenceOtherOptions.Course)
                            {
                                noneFound = false;
                                if (enrollment.Grade != null)
                                    CompletedCourse.Add(courseSequenceOtherOptions);
                                else
                                    InProgressCourse.Add(courseSequenceOtherOptions);
                                courseSequencesRemoved.Remove(courseSequence);
                            }
                            if (noneFound)
                                IncompleteCourse.Add(courseSequence);
                        }
                    } else if (enrollment.Course == courseSequence.Course)
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