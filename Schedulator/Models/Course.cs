using System;
using System.Collections.Generic;
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

        public virtual ICollection<Lecture> Lectures { get; set; }

    }
}