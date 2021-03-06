﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public enum ProgressType { CompletedCourse, InProgressCourse, IncompleteCourse };
    public class Progression
    {
      
        public List<ProgressionUnit> ProgessionUnitList { get; set; }
        public class ProgressionUnit
        {
            public CourseSequence CourseSequence;
            public ProgressType ProgressType;
        }
        public void StudentsProgression(List<Enrollment> studentsEnrollment, List<CourseSequence> courseSequences)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            
            List<Course> basicScienceCourses = db.Courses.AsNoTracking().Where(n => n.ElectiveType == ElectiveType.BasicScience).ToList();
            List<Course> generalCourses = db.Courses.AsNoTracking().Where(n => n.ElectiveType == ElectiveType.GeneralElective).ToList();
            List<CourseSequence> courseSequencesRemoved = courseSequences.ToList();

            
            foreach (Enrollment enrollment in studentsEnrollment)
            {
                foreach(CourseSequence courseSequence in courseSequences)
                {
                    if (courseSequence.OtherOptions.Count() > 0)
                    {
                        foreach (CourseSequence courseSequenceOtherOptions in courseSequence.OtherOptions)
                        {
                            if (courseSequenceOtherOptions.Course != null &&  enrollment.Course.CourseID == courseSequenceOtherOptions.Course.CourseID)
                            {
                                if (enrollment.Grade != null)
                                    ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequenceOtherOptions, ProgressType = ProgressType.CompletedCourse });
                                else
                                    ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequenceOtherOptions, ProgressType = ProgressType.IncompleteCourse });
                                courseSequencesRemoved.Remove(courseSequence);
                            }
                        }
                    } 
                    else if (courseSequence.Course != null && enrollment.Course.CourseID == courseSequence.Course.CourseID)
                    {
                        if (enrollment.Grade != null)
                            ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequence, ProgressType = ProgressType.CompletedCourse });
                        else
                            ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequence, ProgressType = ProgressType.InProgressCourse });
                        courseSequencesRemoved.Remove(courseSequence);
                        break;
                    }
                    else if (courseSequence.ElectiveType == ElectiveType.BasicScience)
                    {
                        foreach ( Course scienceCourse in basicScienceCourses)
                        {
                            if (scienceCourse.CourseID == enrollment.Course.CourseID)
                            {
                                courseSequence.Course = scienceCourse;
                                if (enrollment.Grade != null)
                                    ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequence, ProgressType = ProgressType.CompletedCourse });
                                else
                                    ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequence, ProgressType = ProgressType.InProgressCourse });
                                courseSequencesRemoved.Remove(courseSequence);
                            }
                        }
                        break;
                    }
                    else if (courseSequence.ElectiveType == ElectiveType.GeneralElective)
                    {
                        foreach (Course generalCourse in generalCourses)
                        {
                            if (generalCourse.CourseID == enrollment.Course.CourseID)
                            {
                                courseSequence.Course = generalCourse;
                                if (enrollment.Grade != null)
                                    ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequence, ProgressType = ProgressType.CompletedCourse });
                                else
                                    ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequence, ProgressType = ProgressType.InProgressCourse });
                                courseSequencesRemoved.Remove(courseSequence);
                                break;
                            }
                        }
                        break;
                    }
                    else if (courseSequence.ElectiveType == ElectiveType.TechnicalElective)
                    {
                        foreach(Course technicalCourse in courseSequence.Program.TechnicalElectiveCourses)
                        {
                            if (technicalCourse.CourseID == enrollment.Course.CourseID)
                            {
                                courseSequence.Course = technicalCourse;
                                if (enrollment.Grade != null)
                                    ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequence, ProgressType = ProgressType.CompletedCourse });
                                else
                                    ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = courseSequence, ProgressType = ProgressType.InProgressCourse });
                                courseSequencesRemoved.Remove(courseSequence);
                            }
                        }
                        break;
                    }
                }
                courseSequences = courseSequencesRemoved;
            }
            courseSequences.ForEach(n => ProgessionUnitList.Add(new ProgressionUnit { CourseSequence = n, ProgressType = ProgressType.IncompleteCourse }));
        }

    }
}