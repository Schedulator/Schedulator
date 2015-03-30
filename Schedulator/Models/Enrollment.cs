using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public string Grade { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual Course Course { get; set; }
        public virtual Section Section { get; set; }


    }

   
}