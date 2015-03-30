using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public string CourseLetters { get; set; }
        public int CourseNumber { get; set; }
        public string SpecialNote { get; set; }
        public double Credit { get; set; }
        [DefaultValue(ElectiveType.None)]
        public ElectiveType ElectiveType { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<Prerequisite> Prerequisites { get; set; }


        public List<Prerequisite> MissingPrequisite( List<Enrollment> enrollments)
        {
            List<Prerequisite> missingPrequisite = new List<Prerequisite>();
            foreach (Prerequisite prerequisite in Prerequisites)
            {
                bool enrollmentContainsPrereq = false;
                foreach(Enrollment enrollment in enrollments)
                {
                    if ( prerequisite.Course == enrollment.Course )
                    {
                        enrollmentContainsPrereq = true;
                        break;
                    }
                }
                if (enrollmentContainsPrereq)
                    enrollmentContainsPrereq = false;
                else
                    missingPrequisite.Add(prerequisite);
            }
            return missingPrequisite;
        }
    }
}
