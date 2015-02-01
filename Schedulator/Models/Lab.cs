using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Lab
    {
        public int LabID { get; set; }
        public string LabLetter { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string FirstDay { get; set; }
        public string SecondDay { get; set; }
        public string ClassRoomNumber { get; set; }

        public virtual Lecture Lecture { get; set; }
        public virtual Tutorial Tutorial { get; set; }
    }
}