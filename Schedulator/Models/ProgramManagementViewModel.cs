using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class ProgramManagementViewModel
    {
        public Program Program { get; set; }
        public List<Course> Courses {get; set;}
        public double Credit { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<Prerequisite> Prerequisites { get; set; }


    }
}