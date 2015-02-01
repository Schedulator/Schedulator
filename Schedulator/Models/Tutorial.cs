using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Tutorial
    {
        public int TutorialID { get; set; }
        public string TutorialLetter { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string FirstDay { get; set; }
        public string SecondDay { get; set; }
        public string ClassRoomNumber { get; set; }

        public virtual Lecture Lecture { get; set; }

    }
}