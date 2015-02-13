using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public enum Grade 
    { 
        [Display(Name = "A+")]
        APlus,
        [Display(Name = "A")]
        A,
        [Display(Name = "A-")]
        AMinus,
        [Display(Name = "B+")]
        BPlus,
        [Display(Name = "B")]
        B,
        [Display(Name = "B-")]
        BMinus,
        [Display(Name = "C+")]
        CPlus,
        [Display(Name = "C")]
        C,
        [Display(Name = "C-")]
        CMinus,
        [Display(Name = "D+")]
        DPlus,
        [Display(Name = "D")]
        D,
        [Display(Name = "D-")]
        DMinus,
        [Display(Name = "F")]
        F
    };
    public class Enrollement
    {
        public int EnrollementID { get; set; }
        public Grade Grade { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual Tutorial Tutorial { get; set; }
        public virtual Lab Lab { get; set; }

    }

   
}