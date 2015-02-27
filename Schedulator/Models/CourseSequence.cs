using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class CourseSequence
    {
        public int CourseSequenceId { get; set; }
        public string Season { get; set; }
        public int year { get; set; }

        public virtual Course course { get; set; }
    }
}