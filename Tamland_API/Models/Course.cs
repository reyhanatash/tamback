using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tamland_API.Models
{
    public class Course
    {
        enum ResponseType
        {
            Ok = 200,
            NotOk = 400
        }

        public class Response
        {
            public string Message { get; set; }
            public int Code { get; set; }
            public object Data { get; set; }
        }

        public class Courses
        {
            public int CourseId { get; set; }
            public int Deleted { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Keyword { get; set; }
            public int IsActive { get; set; }
            public DateTime PublishDate { get; set; }
            public int Type { get; set; }
            public int Price { get; set; }
            public int IsOffered { get; set; }
            public int Index { get; set; }
            public int PriceRegion1 { get; set; }
            public int PriceRegion2 { get; set; }
            public int PriceRegion3 { get; set; }
            public DateTime StartDateTime { get; set; }
            public int System { get; set; }
            public int StepCount { get; set; }
            public string CourseStartDescription { get; set; }
            public int TeacherId { get; set; }
            public int StageId { get; set; }
        }

        public class CourseStep
        {
            public int CourseId { get; set; }
            public int CourseStepId { get; set; }
            public int Deleted { get; set; }
            public string Title { get; set; }
            public string Keyword { get; set; }
            public int Type { get; set; }
            public int IsFree { get; set; }
            public int Index { get; set; }
            public DateTime EndDateTime { get; set; }
            public DateTime StartDateTime { get; set; }
            public int HasFile { get; set; }
            public string FileUrl { get; set; }
            public int IsEnded { get; set; }
            public int IsStarted { get; set; }
            public string VideoSource { get; set; }
            public string BaseCourseStepCo { get; set; }
            public string StreamId { get; set; }
            public int AudienceId { get; set; }

        }

        public class State
        {
            public int StateId { get; set; }
            public int CityId { get; set; }
            public long VillageId { get; set; }
        }
        public class TestModel
        {
            public string Id { get; set; }
            public string Question { get; set; }
            public string Answer1 { get; set; }
            public string Answer2 { get; set; }
            public string Answer3 { get; set; }
            public string Answer4 { get; set; }
            public string Answer5 { get; set; }
            public int CourseId { get; set; }
        }
        public class TestAnswerModel
        {
            public string TestId { get; set; }
            public int Answer { get; set; }
        }

        public class Teacher
        {
            public int TeacherId { get; set; }
        }

        public class Package
        {
            public int PackageId { get; set; }
            public int CourseId { get; set; }
            public int Deleted { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime StartDateTime { get; set; }
            public DateTime PublishDate { get; set; }
            public int IsActive { get; set; }
            public int IsOffered { get; set; }
            public int Index { get; set; }
            public int AudienceId { get; set; }
            public int StageId { get; set; }
            public int System { get; set; }
            public int Type { get; set; }
            public int DiscountPercentage { get; set; }
        }

    }
}