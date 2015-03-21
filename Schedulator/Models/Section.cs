using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(false)]
        public bool SectionMaster { get; set; }


        public virtual Section OtherSimilarSectionMaster { get; set; } // groups same lecture and same tutorial and lab time
        public virtual ICollection<Section> OtherSimilarSections { get; set; }
    }
}