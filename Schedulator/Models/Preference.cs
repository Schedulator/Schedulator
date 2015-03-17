using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Preference
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public ICollection<String> Days { get; set; }
        public ICollection<Course> Courses { get; set; }
        public bool UseCourseSequence { get; set; }
        [DefaultValueAttribute(5)]
        public int NumberOfCourses { get; set; }


    }
}