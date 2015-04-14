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
        public Semester Semester { get; set; }
        public ICollection<String> Days { get; set; }
        public List<Course> Courses { get; set; }
    }
}