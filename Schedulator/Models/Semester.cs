using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public enum Season 
    {
        Fall, Winter, Summer1,Summer2
    }
    public class Semester
    {
        public int SemesterID { get; set; }
        public Season Season { get; set; }
        public DateTime SemesterStart {get; set;}
        public DateTime SemesterEnd { get; set; }
    }
}