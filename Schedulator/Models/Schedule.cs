using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual ApplicationUser ApplicationUser {get; set;}
        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}