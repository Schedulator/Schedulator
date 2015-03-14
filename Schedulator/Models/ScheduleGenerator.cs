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
        public ICollection<PrequisitesStudentNeedsForCourse> PrequisitesStudentNeedsForCourses { get; set; }
        

        public class PrequisitesStudentNeedsForCourse
        {
            public Course Course { get; set; }
            public ICollection<Prerequisite> PrequisitesStudentNeeds { get; set; }
        }
        public void GenerateSchedules(List<Course> courses, List<Enrollment> enrollments, Program program)
        {
            List<Course> coursesStudentCanTake = new List<Course>();

            PrequisitesStudentNeedsForCourses = new List<PrequisitesStudentNeedsForCourse>();
            AddUserPreferenceCourses(courses, enrollments, program, coursesStudentCanTake);


        }
        public List<Course> AddUserPreferenceCourses(List<Course> courses, List<Enrollment> enrollments, Program program, List<Course> coursesStudentCanTake)
        {
            if (Preference.Courses.Count > 0) // 
            {
                foreach (Course course in Preference.Courses)
                {
                    List<Prerequisite> prerequisitesStudentNeeds = course.MissingPrequisite(enrollments);
                    if (prerequisitesStudentNeeds.Count == 0) // Check if student has all prerquisite for the course they want to add
                        coursesStudentCanTake.Add(course);
                    else // If they don't then add it to class so we can tell the user what course they can't take and what prerequisites they need
                        PrequisitesStudentNeedsForCourses.Add(new PrequisitesStudentNeedsForCourse { Course = course, PrequisitesStudentNeeds = prerequisitesStudentNeeds });
                }
            }
            return coursesStudentCanTake;
        }
        public void AddUsersCourseSequence(List<Course> courses, List<Enrollment> enrollments, Program program, List<Course> coursesStudentCanTake)
        {
            if (Preference.UseCourseSequence) // Generate from Program sequence passed
            {
                List<CourseSequence> courseSequenceToTake = new List<CourseSequence>();
                foreach (CourseSequence courseSequence in program.courseSequences.OrderBy(p => p.CourseSequenceId))
                {
                    foreach (Enrollment enrollment in enrollments)
                    {
                        if (enrollment.Course == courseSequence.Course) // Check if student has taken the course already
                            break;
                        else if (courseSequence.Course.MissingPrequisite(enrollments).Count == 0) // if the student hasn't taken the course already check that he has prereqs
                            courseSequenceToTake.Add(courseSequence);
                        if (courseSequenceToTake.Count >= 5)
                        {
                            foreach (CourseSequence courseSequenceSameSemester in courseSequenceToTake)
                            {

                            }
                        }
                    }

                }
            }
        }
    }
}