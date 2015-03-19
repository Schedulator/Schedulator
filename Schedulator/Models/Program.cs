using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Program
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string ProgramOption { get; set; }
        public int CreditsRequirement { get; set; }
        public Season Season { get; set; }

        public virtual ICollection<CourseSequence> courseSequences { get; set; }
    }
}