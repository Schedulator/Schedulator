using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Prerequisite
    {
        public int PrerequisiteID { get; set; }
        public virtual Course Course {get; set;}
        public virtual Course PrerequisiteCourse { get; set; }
    }
}