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
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public string Grade { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual Course Course { get; set; }
        public virtual Section Section { get; set; }


        public string CreateDivBlock()
        {
            string htmlString = "";
            htmlString += GenerateHtmDivBlock(Convert.ToInt32(this.Section.Lecture.EndTime - this.Section.Lecture.StartTime), FindHorizontalBlockLocation(this.Section.Lecture.FirstDay), this.Section.Lecture.StartTime, this.Section.Lecture.EndTime, "Lect", this.Section.Lecture.LectureLetter);
            if ( this.Section.Lecture.SecondDay != null)
                htmlString += GenerateHtmDivBlock(Convert.ToInt32(this.Section.Lecture.EndTime - this.Section.Lecture.StartTime), FindHorizontalBlockLocation(this.Section.Lecture.SecondDay), this.Section.Lecture.StartTime, this.Section.Lecture.EndTime, "Lect", this.Section.Lecture.LectureLetter);

            if (this.Section.Tutorial != null)
            {
                htmlString += GenerateHtmDivBlock(Convert.ToInt32(this.Section.Tutorial.EndTime - this.Section.Tutorial.StartTime), FindHorizontalBlockLocation(this.Section.Tutorial.FirstDay), this.Section.Tutorial.StartTime, this.Section.Tutorial.EndTime, "Tut", this.Section.Tutorial.TutorialLetter);
                  if ( this.Section.Tutorial.SecondDay != null)
                      htmlString += GenerateHtmDivBlock(Convert.ToInt32(this.Section.Tutorial.EndTime - this.Section.Tutorial.StartTime), FindHorizontalBlockLocation(this.Section.Tutorial.SecondDay), this.Section.Tutorial.StartTime, this.Section.Tutorial.EndTime, "Tut", this.Section.Tutorial.TutorialLetter);
            }

            if (this.Section.Lab != null)
            {
                htmlString += GenerateHtmDivBlock(Convert.ToInt32(this.Section.Lab.EndTime - this.Section.Lab.StartTime), FindHorizontalBlockLocation(this.Section.Lab.FirstDay), this.Section.Lab.StartTime, this.Section.Lab.EndTime, "Tut", this.Section.Lab.LabLetter);
                if (this.Section.Lab.SecondDay != null)
                    htmlString += GenerateHtmDivBlock(Convert.ToInt32(this.Section.Lab.EndTime - this.Section.Lab.StartTime), FindHorizontalBlockLocation(this.Section.Lab.SecondDay), this.Section.Lab.StartTime, this.Section.Lab.EndTime, "Tut", this.Section.Lab.LabLetter);
            }
            return htmlString;
        }
        private string GenerateHtmDivBlock(int height, int blockLocation, double startTime, double endTime, string type, string typeLetter)
        {
            if (height < 75)
                height = 75;
            string htmlDivString = "";
            htmlDivString += "<div id=\"" + type + "\" style=\"height:" + height + "px;left:" + blockLocation + "%;top:" + (startTime - 420) + "px\">";
            htmlDivString += "<p align=\"center\">" + this.Course.CourseLetters + " " + this.Course.CourseNumber
                         + "<br/> " + type + " " + typeLetter + "<br/>"
                         + TimeSpan.FromMinutes(startTime).ToString(@"hh\:mm") + " - " + TimeSpan.FromMinutes(endTime).ToString(@"hh\:mm")
                         + "</div>";
            return htmlDivString;
        }
        private int FindHorizontalBlockLocation (Schedulator.Models.TimeBlock.day day)
        {
            return (int)day * 20;
            //switch (day)
            //{
            //    case 0:
            //        return 0;
            //    case 1:
            //        return 20;
            //    case :
            //        return 40;
            //    case "J":
            //        return 60;
            //    case "F":
            //        return 80;
                    
            //}
            //return 0;
        }
    }

   
}