using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class ProgramDirector
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<Course> detailedCourseList = new List<Course>();

        public List<Course> getProgramCourseIDs(int programId)
        {
            bool containsItem = false;
            var coursesList = db.Courses.ToList().Select(a => new Course { CourseID = a.CourseID, Title = a.Title, CourseLetters = a.CourseLetters, CourseNumber = a.CourseNumber, SpecialNote = a.SpecialNote }).ToList();
            var courseSeqIds = db.CourseSequence.ToList().Where(n => n.Program.ProgramId == programId && n.Course != null).ToList();
            var courseIdsList = courseSeqIds.Select(t => t.Course.CourseID).ToList();

            for (int j = 0; j < coursesList.Count; j++)
            {
                for (int i = 0; i < courseSeqIds.Count; i++)
                {
                    containsItem = detailedCourseList.Any(item => item.CourseID == courseIdsList[i]);

                    if (coursesList[j].CourseID == courseIdsList[i] && containsItem == false)
                    {
                        detailedCourseList.Add(coursesList[j]);
                    }
                }
            }
            return detailedCourseList;

        }
        
        public List<Lecture> getLecturesFromCourse(Course course)
        {
            var courseId = course.CourseID;
            var courseIds = db.Courses.Select(t => t.CourseID).ToList();
            var lectureIds = db.Lectures.Where(v => v.LectureLetter != "UgradNSched IE").Select(t => t.LectureID).ToList();
            //var lectures = db.Lectures.Where(x => x.Course.CourseID == course.CourseID && x.LectureLetter != "UgradNSched IE").ToList();

            List<Lecture> lecList = new List<Lecture>();

            var lectureList = db.Lectures.Where(n => n.LectureLetter != "UgradNSched IE").ToList();

            bool containsItem = false;

            for (int i = 0; i < lectureList.Count; i++)
            {

                containsItem = lecList.Any(item => item.LectureID == lectureIds[i]);

                if (containsItem == false)
                {
                    if (lectureList[i].Course.CourseID == courseId)
                    {
                        lecList.Add(lectureList[i]);
                    }
                }

            }
            
            return lecList;
        }
        
        public List<Tutorial> getTutorialsFromCourse(Tutorial course)
        {
            var tutList = db.Tutorials.Where(n => n.TutorialID == course.TutorialID).ToList();
            return tutList;
        }

        public List<Lab> getLabsFromCourse(Lab course)
        {
            var labList = db.Labs.Where(n => n.LabID == course.LabID).ToList();
            return labList;
        }
     
    }
}