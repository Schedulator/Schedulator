using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    
    public class TimeBlock
    {
        public enum day {M, T, W, J, F, S, D,NONE};

        public double StartTime { get; set; }
        public double EndTime { get; set; }
        [DefaultValue(day.NONE)]
        public day FirstDay { get; set; }
        [DefaultValue(day.NONE)]
        public day SecondDay { get; set; }
    }
}