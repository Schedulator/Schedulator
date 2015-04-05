using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Enrollment : IValidatableObject
    {
        public int EnrollmentID { get; set; }
        public string Grade { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual Course Course { get; set; }
        public virtual Section Section { get; set; }

        public double getGPA()
        {
            switch (Grade)
            {
                case "A+" : return 4.3;
                case "A"  : return 4.0;
                case "A-" : return 3.7;
                case "B+" : return 3.3;
                case "B"  : return 3.0;
                case "B-" : return 2.7;
                case "C+" : return 2.3;
                case "C"  : return 2.0;
                case "C-" : return 1.7;
                case "D+" : return 1.3;
                case "D"  : return 1.0;
                case "D-" : return 0.7;
                default   : return 0.0;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(new string[] {"A+","A","A-","B+","B","B-","C+","C","C-","D+","D","D-", null}.Contains(Grade)))
                
            {
                yield return new ValidationResult("Grade is invalid");
            }
        }
    }

   
}