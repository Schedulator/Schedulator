using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedulator.Models
{
    public class ScheduleGenerator
    {
        public Preference Preference { get; set; }
        public List<List<Schedule>> Schedules { get; set; }
        public List<PrequisitesStudentNeedsForCourse> PrequisitesStudentNeedsForCourses { get; set; }
        public string MessageToUser = "";

        private List<Course> CoursesStudentWantAndCanTake = new List<Course>();
        private List<CourseSequence> MainCourseSequencesToTake = new List<CourseSequence>();
        private List<CourseSequence> SecondaryCourseSequencesToTake = new List<CourseSequence>();
        public class PrequisitesStudentNeedsForCourse
        {
            public Course Course { get; set; }
            public List<Prerequisite> PrequisitesStudentNeeds { get; set; }
        }
        public void GenerateSchedules(List<Course> courses, List<Enrollment> enrollments, Program program)
        {
            PrequisitesStudentNeedsForCourses = new List<PrequisitesStudentNeedsForCourse>();
            if (!Preference.UseCourseSequence)
            {
                AddUserPreferenceCourses(courses, enrollments, program);
                GenerateAllSchedulesUsingUserPreferenceCourses();
            }  
        }
        public void GenerateAllSchedulesUsingUserPreferenceCourses()
        {
            ApplicationDbContext db = new ApplicationDbContext();
          
            List<List<Section>> sectionsListMaster = new List<List<Section>>();

            List<Section> sectionns = db.Section.Where(n => ( n.Lecture.Semester.Season == Season.Summer2) && n.Lecture.Course.CourseNumber == 282 && n.Lecture.Course.CourseLetters == "ENCS" ).ToList();
            if (Preference.StartTime == 0 && Preference.EndTime == 0)
            {
                if (Preference.Semester.Season == Season.Summer1 || Preference.Semester.Season == Season.Summer2)
                {
                    foreach (Course course in CoursesStudentWantAndCanTake)
                    {
                        sectionsListMaster.Add(db.Section.Where(n => (n.Lecture.Semester.Season == Season.Summer1 || n.Lecture.Semester.Season == Season.Summer2) && n.Lecture.Course.CourseID == course.CourseID && n.OtherSimilarSectionMaster == null).ToList());
                    }
                }
                else
                {

                    foreach (Course course in CoursesStudentWantAndCanTake)
                    {
                        sectionsListMaster.Add(db.Section.Where(n => n.Lecture.Semester.Season == Preference.Semester.Season && n.Lecture.Course.CourseID == course.CourseID && n.OtherSimilarSectionMaster == null).ToList());
                    }
                }
            }
            else
                foreach (Course course in CoursesStudentWantAndCanTake)
                    sectionsListMaster.Add(db.Section.Where(n => n.Lecture.Semester == Preference.Semester && (n.Lecture.Course == course
                        && n.Lecture.StartTime <= Preference.StartTime && n.Lecture.EndTime >= Preference.EndTime
                        && n.Tutorial.StartTime <= Preference.StartTime && n.Tutorial.EndTime >= Preference.EndTime
                        && n.Lab.StartTime <= Preference.StartTime && n.Lab.EndTime >= Preference.EndTime)).ToList());

            List<List<Section>>  sectionList = new List<List<Section>>();
            GetAllValidSectionCombination(sectionsListMaster, 0, new List<Section>(), sectionList);

            Schedules = new List<List<Schedule>>();
            foreach(List<Section> sectionsForSchedule in sectionList )
            {
                Schedules.Add(new List<Schedule>());
               
                Schedules.LastOrDefault().Add(new Schedule {Enrollments = new List<Enrollment>(), Semester = Preference.Semester });
                foreach( Section sectionForSchedule in sectionsForSchedule)
                {
                    if (sectionForSchedule.Lecture.Semester.Season == Season.Summer2)
                    {
                        if (Schedules.LastOrDefault().Count == 1)
                            Schedules.LastOrDefault().Add(new Schedule { Enrollments = new List<Enrollment>(), Semester = db.Semesters.Where(n => n.Season == Season.Summer2).FirstOrDefault()});
                        Schedules.LastOrDefault().LastOrDefault().Enrollments.Add(new Enrollment { Course = sectionForSchedule.Lecture.Course, Section = sectionForSchedule, Schedule = Schedules.LastOrDefault().LastOrDefault() });
                    }
                    else
                        Schedules.LastOrDefault().FirstOrDefault().Enrollments.Add(new Enrollment { Course = sectionForSchedule.Lecture.Course, Section = sectionForSchedule, Schedule = Schedules.LastOrDefault().LastOrDefault() });
                }
            }

        }
        public class HoldStartAndEndTime
        {
            public double StartTime;
            public double EndTime;
            public Schedulator.Models.TimeBlock.day FirstDay;
            public Schedulator.Models.TimeBlock.day SecondDay;
        }
        public bool CheckIfTimeConflict (List<Section> sections, Section sectionToAdd)
        {
            
            List<HoldStartAndEndTime> sectionToAddTimes = new List<HoldStartAndEndTime>() { 
                new HoldStartAndEndTime { StartTime = sectionToAdd.Lecture.StartTime, EndTime = sectionToAdd.Lecture.EndTime, FirstDay = sectionToAdd.Lecture.FirstDay, SecondDay = sectionToAdd.Lecture.SecondDay}};
                if (sectionToAdd.Tutorial != null )
                    sectionToAddTimes.Add(new HoldStartAndEndTime { StartTime = sectionToAdd.Tutorial.StartTime, EndTime = sectionToAdd.Tutorial.EndTime, FirstDay = sectionToAdd.Tutorial.FirstDay, SecondDay = sectionToAdd.Tutorial.SecondDay});
                if (sectionToAdd.Lab != null)    
                    sectionToAddTimes.Add(new HoldStartAndEndTime { StartTime = sectionToAdd.Lab.StartTime, EndTime = sectionToAdd.Lab.EndTime, FirstDay = sectionToAdd.Lab.FirstDay, SecondDay = sectionToAdd.Lab.SecondDay});
            
            foreach(Section section in sections)
            {
                if (section.Lecture.Semester.Season == sectionToAdd.Lecture.Semester.Season)
                {
                    List<HoldStartAndEndTime> sectionTimes = new List<HoldStartAndEndTime>(){
                    new HoldStartAndEndTime { StartTime = section.Lecture.StartTime, EndTime = section.Lecture.EndTime, FirstDay = section.Lecture.FirstDay, SecondDay = section.Lecture.SecondDay}};
                    if (section.Tutorial != null)
                        sectionTimes.Add(new HoldStartAndEndTime { StartTime = section.Tutorial.StartTime, EndTime = section.Tutorial.EndTime, FirstDay = section.Tutorial.FirstDay, SecondDay = section.Tutorial.SecondDay });
                    if (sectionToAdd.Lab != null)
                        sectionTimes.Add(new HoldStartAndEndTime { StartTime = section.Lab.StartTime, EndTime = section.Lab.EndTime, FirstDay = section.Lab.FirstDay, SecondDay = section.Lab.SecondDay });

                    foreach (HoldStartAndEndTime sectionToAddTime in sectionToAddTimes)
                    {
                        foreach (HoldStartAndEndTime sectionTime in sectionTimes)
                        {
                            if (CheckIfSameDays(sectionToAddTime.FirstDay, sectionToAddTime.SecondDay, sectionTime.FirstDay, sectionTime.SecondDay) && CheckIfTimeOverlap(sectionToAddTime.StartTime, sectionToAddTime.EndTime, sectionTime.StartTime, sectionTime.EndTime))
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool CheckIfSameDays(Schedulator.Models.TimeBlock.day firstDay, Schedulator.Models.TimeBlock.day secondDay, Schedulator.Models.TimeBlock.day secondFirstDay, Schedulator.Models.TimeBlock.day secondSecondDay)
        {
            if (firstDay == secondFirstDay || (secondSecondDay != TimeBlock.day.NONE && firstDay == secondSecondDay))
                return true;
            else if (secondDay != TimeBlock.day.NONE && (secondDay == secondFirstDay || secondDay == secondSecondDay))
                return true;
            else
                return false;
        }
        public bool CheckIfTimeOverlap (double startTime, double endTime, double secondStartTime, double secondEndTime)
        {
            if (secondStartTime >= startTime && secondStartTime < endTime)
                return true;
            else if (secondEndTime > startTime && secondEndTime <= endTime)
                return true;
            else if (startTime >= secondStartTime && startTime < secondEndTime)
                return true;
            else if (endTime > secondStartTime && endTime <= secondEndTime)
                return true;
            else
                return false;
        }
        public void AddUserPreferenceCourses(List<Course> courses, List<Enrollment> enrollments, Program program)
        {
            if (Preference.Courses.Count > 0) // 
            {
                foreach (Course course in Preference.Courses)
                {
                    List<Prerequisite> prerequisitesStudentNeeds = course.MissingPrequisite(enrollments);
                    if (prerequisitesStudentNeeds.Count == 0) // Check if student has all prerquisite for the course they want to add
                        CoursesStudentWantAndCanTake.Add(course);
                    else // If they don't then add it to class so we can tell the user what course they can't take and what prerequisites they need
                        PrequisitesStudentNeedsForCourses.Add(new PrequisitesStudentNeedsForCourse { Course = course, PrequisitesStudentNeeds = prerequisitesStudentNeeds });
                }
            }

        }
        void GetAllValidSectionCombination(List<List<Section>> keys, int index, List<Section> values, List<List<Section>> sectionsLists)
        {
            List<Section> key = keys[index];
            List<List<Section>> lists = new List<List<Section>>();
            foreach (Section section in key)
            {
                if (!CheckIfTimeConflict(values.GetRange(0,index), section))
                {
                    if (values.Count() == index)
                        values.Add(section);
                    else
                        values[index] = (section);

                    if (index < keys.Count() - 1)
                    {
                        GetAllValidSectionCombination(keys, index + 1, values, sectionsLists);
                    }
                    else
                    {
                        sectionsLists.Add(values.ToList()); // Clone the array;
                    }
                }
            }
        }
    }
}