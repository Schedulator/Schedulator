using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class ScheduleGenerator
    {
        public Preference Preference { get; set; }
        public ICollection<Schedule> Schedules { get; set; }   
    }
}