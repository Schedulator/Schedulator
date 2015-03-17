using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Student
    {
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}