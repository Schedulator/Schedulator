using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public enum ElectiveType
    {
        None, BasicScience,GeneralElective,TechnicalElective 
    }
    public class CourseSequence
    {
        public int CourseSequenceId { get; set; }
        public Season Season { get; set; }
        public int Year { get; set; }
        public ElectiveType ElectiveType { get; set; }
        public virtual Program Program { get; set; }
        public virtual Course Course { get; set; }
        public virtual CourseSequence ContainerSequence { get; set; }
        public virtual ICollection<CourseSequence> OtherOptions { get; set; }
    }
}