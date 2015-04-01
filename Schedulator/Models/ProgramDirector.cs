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

                    if (coursesList[j].CourseID == courseIdsList[i] && courseIdsList[i] != null && containsItem == false)
                    {
                        detailedCourseList.Add(coursesList[j]);
                    }
                }
            }
            return detailedCourseList;

        }
        /*
        public ICollection<Lecture> getLecturesFromCourses()
        {
        
                    
        
        }

        public ICollection<Prerequisite> getPrerequisitesFromCourses()
        {


            //return ICollection<Prerequisite> prereqList;
        }
         */
    }
}