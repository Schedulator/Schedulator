using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Tutorial : TimeBlock
    {
        public int TutorialID { get; set; }
        public string TutorialLetter { get; set; }
        public string ClassRoomNumber { get; set; }

        public virtual Lecture Lecture { get; set; }
        public virtual ICollection<Lab> Labs { get; set; }

    }
}