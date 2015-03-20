using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Section
    {
        public int SectionId { get; set; }

        public virtual Lecture Lecture { get; set; }
        public virtual Tutorial Tutorial { get; set; }
        public virtual Lab Lab { get; set; }

        public virtual ICollection<Section> OtherSimilarSections { get; set; } // Same lecture and same tutorial and lab time
    }
}