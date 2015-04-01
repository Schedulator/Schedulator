using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Schedulator.Models
{
    using EnumExtension;
    public enum ElectiveType
    {
        [Display(Name = "None")]
        None,
        [Display(Name = "Basic Science")]
        BasicScience,
        [Display(Name = "General Elective")]
        GeneralElective,
        [Display(Name = "Technical Elective")]
        TechnicalElective 

    };
    public class CourseSequence
    {
       

        public int CourseSequenceId { get; set; }
        public Season Season { get; set; }
        public int Year { get; set; }
        public ElectiveType ElectiveType { get; set; }
        public virtual Program Program { get; set; }
        public virtual Course Course { get; set; }
        public virtual CourseSequence ContainerSequence { get; set; }
        public virtual List<CourseSequence> OtherOptions { get; set; }


    }



}
namespace EnumExtension
{
    public static class Extensions
    {
        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }

    }
}