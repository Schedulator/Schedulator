using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Prerequisite
    {
        public int PrerequisiteID { get; set; }
        public bool Concurrently { get; set; }
        public virtual Course Course {get; set;}
        public virtual Course PrerequisiteCourse { get; set; }
        public virtual Course OrPrerequisiteCourse { get; set; }
    }
}