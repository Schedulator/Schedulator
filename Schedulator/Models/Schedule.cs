﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public bool IsRegisteredSchedule { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ApplicationUser ApplicationUser {get; set;}
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        [NotMapped]
        public List<TimeBlock.day> Days = new List<TimeBlock.day>();
        
        public void FillDayList()
        {
            foreach (Enrollment enrollment in Enrollments)
            {
                if (Days.Contains(enrollment.Section.Lecture.FirstDay))
                    Days.Add(enrollment.Section.Lecture.FirstDay);
                if (Days.Contains(enrollment.Section.Lecture.SecondDay))
                    Days.Add(enrollment.Section.Lecture.SecondDay);
                if (enrollment.Section.Tutorial != null)
                {
                    if (Days.Contains(enrollment.Section.Tutorial.FirstDay))
                        Days.Add(enrollment.Section.Tutorial.FirstDay);
                    if (Days.Contains(enrollment.Section.Tutorial.FirstDay))
                        Days.Add(enrollment.Section.Tutorial.FirstDay);
                }
                if (enrollment.Section.Lab != null)
                {
                    if (Days.Contains(enrollment.Section.Lab.FirstDay))
                        Days.Add(enrollment.Section.Lab.FirstDay);
                    if (Days.Contains(enrollment.Section.Lab.FirstDay))
                        Days.Add(enrollment.Section.Lab.FirstDay);
                }
                if (Days.Count() == 5)
                    return;
            }
        }
        public void RemoveCourseFromSchedule(List<int> sectionIds, ApplicationDbContext db)
        {
            foreach (int sectionId in sectionIds)
            {
                Enrollment enrollment = this.Enrollments.Where(n => n.Section.SectionId == sectionId).FirstOrDefault();
                if (enrollment != null)
                {
                    this.Enrollments.Remove(enrollment);
                    db.Enrollment.Remove(enrollment);

                }
                db.Entry(this).State = System.Data.Entity.EntityState.Modified;
            }
        }
        public bool RegisterSchedule()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Prerequisite> missingPrequisite = new List<Prerequisite>();
            Schedule currentStudentsSchedule;
            if (IsRegisteredSchedule)
            {
                currentStudentsSchedule = db.Schedule.Where(n => n.ApplicationUser.Id == ApplicationUser.Id && n.Semester.SemesterID == Semester.SemesterID && n.IsRegisteredSchedule == true).FirstOrDefault();

                if (currentStudentsSchedule != null)
                {
                    db.Enrollment.RemoveRange(currentStudentsSchedule.Enrollments);
                    db.Schedule.Remove(currentStudentsSchedule);
                    db.SaveChanges();
                }
            }
         //   foreach(Enrollment enrollmentToRegister in Enrollments )
        //    {
       //         missingPrequisite.AddRange(enrollmentToRegister.Course.MissingPrequisite(studentsEnrollments));
       //     }

            return true;
        }

        public string RenderSchedule()
        {
            List<Enrollment> enrollments = Enrollments.ToList();
            string htmlToReturn = "";
            
            double earliestTime = 100000;
            double latestTime = 0;
            foreach (Enrollment enrollment in enrollments)
            {
                if (enrollment.Section.Lecture.StartTime < earliestTime)
                    earliestTime = enrollment.Section.Lecture.StartTime;
                if (enrollment.Section.Tutorial != null && enrollment.Section.Tutorial.StartTime < earliestTime)
                    earliestTime = enrollment.Section.Tutorial.StartTime;
                if (enrollment.Section.Lab != null && enrollment.Section.Lab.StartTime < earliestTime)
                    earliestTime = enrollment.Section.Lab.StartTime;

                if (enrollment.Section.Lecture.EndTime > latestTime)
                    latestTime = enrollment.Section.Lecture.EndTime;
                if (enrollment.Section.Tutorial != null && enrollment.Section.Tutorial.EndTime > latestTime)
                    latestTime = enrollment.Section.Tutorial.EndTime;
                if (enrollment.Section.Lab != null && enrollment.Section.Lab.EndTime > latestTime)
                    latestTime = enrollment.Section.Lab.EndTime;
            }

            earliestTime = 15 * (int)earliestTime / 15;
            latestTime = 15 * (int)latestTime / 15;
            int numberOfRows = (int)(latestTime - earliestTime) / 15;
            ScheduleBlock[][] scheduleBlocks = new ScheduleBlock[numberOfRows][];

            for (int i = 0; i < numberOfRows; i++){
                scheduleBlocks[i] = new ScheduleBlock[5];
                for (int k = 0; k < 5; k++)
                    scheduleBlocks[i][k] = new ScheduleBlock() { RenderType = BlockRenderType.EMPTY };
            }

            foreach(Enrollment enrollment in enrollments){

                AddLectureTutorialLabToScheduleBlock(enrollment.Section.Lecture.FirstDay,
                                                     enrollment.Section.Lecture.SecondDay, 
                                                     enrollment.Section.Lecture.StartTime, 
                                                     enrollment.Section.Lecture.EndTime, 
                                                     earliestTime, 
                                                     "Lec", 
                                                     enrollment.Section.Lecture.LectureID, 
                                                     enrollment.Section.Lecture.Course.CourseLetters, 
                                                     enrollment.Section.Lecture.Course.CourseNumber, 
                                                     scheduleBlocks);

                if ( enrollment.Section.Tutorial != null)
                    AddLectureTutorialLabToScheduleBlock(enrollment.Section.Tutorial.FirstDay, 
                                                         enrollment.Section.Tutorial.SecondDay, 
                                                         enrollment.Section.Tutorial.StartTime, 
                                                         enrollment.Section.Tutorial.EndTime, 
                                                         earliestTime, 
                                                         "Tut", 
                                                         enrollment.Section.Lecture.LectureID, 
                                                         enrollment.Section.Lecture.Course.CourseLetters, 
                                                         enrollment.Section.Lecture.Course.CourseNumber, 
                                                         scheduleBlocks);
                if (enrollment.Section.Lab != null)
                    AddLectureTutorialLabToScheduleBlock(enrollment.Section.Lab.FirstDay, 
                                                         enrollment.Section.Lab.SecondDay, 
                                                         enrollment.Section.Lab.StartTime, 
                                                         enrollment.Section.Lab.EndTime, 
                                                         earliestTime, 
                                                         "Lab", 
                                                         enrollment.Section.Lecture.LectureID, 
                                                         enrollment.Section.Lecture.Course.CourseLetters, 
                                                         enrollment.Section.Lecture.Course.CourseNumber, 
                                                         scheduleBlocks);
    
            }

           int currentTime = (int)earliestTime;
           for (int i = 0; i < numberOfRows; i++){

               htmlToReturn += "<tr><td id=\"time\"> " + TimeSpan.FromMinutes(currentTime).ToString(@"hh\:mm") + "</td>";
                    
               for (int k = 0; k < 5; k++)
                   htmlToReturn += scheduleBlocks[i][k].GetBlockHtml();
               currentTime += 15;
               htmlToReturn += "</tr>";
           }
           return htmlToReturn;
            
        }

        public void AddLectureTutorialLabToScheduleBlock(TimeBlock.day firstDay, 
                                                         TimeBlock.day secondDay, 
                                                         double startTime, 
                                                         double endTime, 
                                                         double earliestTime, 
                                                         string blockType, 
                                                         int blockId, 
                                                         string blockLetters, 
                                                         int blockNumber, 
                                                         ScheduleBlock[][] scheduleBlocks){

            int row = (int)(startTime - earliestTime) / 15;
            int firstColumn = (int)firstDay;
            int rowSpawn = (int)(endTime - startTime) / 15;
            scheduleBlocks[row][firstColumn] = new ScheduleBlock() { RowSpawn = rowSpawn, 
                                                                     RenderType = BlockRenderType.DATA, 
                                                                     BlockType = blockType, 
                                                                     BlockId = blockId, 
                                                                     BlockLetters = blockLetters, 
                                                                     BlockNumber = blockNumber,
                                                                     StartTime = startTime,
                                                                     EndTime = endTime
                                                                    };

            
            for (int i = 1; i < rowSpawn; i++)
                scheduleBlocks[row + i][firstColumn] = new ScheduleBlock() { RenderType = BlockRenderType.NONE };

            int secondColumn;

            if (secondDay != TimeBlock.day.NONE){
                secondColumn = (int)secondDay;
                scheduleBlocks[row][secondColumn] = new ScheduleBlock() { RowSpawn = rowSpawn, 
                                                                          RenderType = BlockRenderType.DATA, 
                                                                          BlockType = blockType, 
                                                                          BlockId = blockId, 
                                                                          BlockLetters = blockLetters, 
                                                                          BlockNumber = blockNumber,
                                                                          StartTime = startTime,
                                                                          EndTime = endTime
                                                                        };

                for (int i = 1; i < rowSpawn; i++)
                    scheduleBlocks[row + i][secondColumn] = new ScheduleBlock() { RenderType = BlockRenderType.NONE };
            }
        }

    }
    
    public enum BlockRenderType { EMPTY, NONE, DATA }
    public class ScheduleBlock
    {
        public BlockRenderType RenderType { get; set; } 
        public int BlockNumber { get; set; }
        public int RowSpawn {get; set;}
        public string BlockLetters { get; set; }
        public string BlockType { get; set; }
        public int BlockId { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public string GetBlockHtml()
        {
            if (RenderType == BlockRenderType.EMPTY)
                return "<td>&nbsp</td>";
            else if (RenderType == BlockRenderType.DATA)
                return "<td class =\"" + BlockLetters + BlockNumber + "\" rowspan=\"" + RowSpawn + 
                        "\"  style=\"cursor:pointer;font-size:12px;\" tilte=\"Test\" data-toggle=\"tooltip\" title=\" " +
                        TimeSpan.FromMinutes(StartTime).ToString(@"hh\:mm") + " - " + TimeSpan.FromMinutes(EndTime).ToString(@"hh\:mm") + "\" >" + BlockLetters + "<br>" + BlockNumber + "<br>" + BlockType + "</td>";
            else
                return "";
        }

    }


}