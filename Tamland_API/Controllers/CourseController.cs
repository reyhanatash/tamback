using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Security.Claims;
using static Tamland_API.Models.Course;


namespace Tamland_API.Controllers
{
    [RoutePrefix("api/course")]
    public class CourseController : ApiController
    {

        enum ResponseType
        {
            Ok = 200,
            NotOk = 400
        }

        [HttpGet]
        [Authorize]
        [Route("load")]
        public IHttpActionResult CourseLoad()
        {
            var result = new Response();
            try
            {

                var res = ClsDatabase.ExecuteDatatableSP("SpCourseLoad",
                ClsDatabase.GenParameters(0), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("sessionLoad")]
        public IHttpActionResult SessionLoad(Courses data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SpCourseStepLoad",
                ClsDatabase.GenParameters("@CourseCo", data.CourseId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("loadUserCourse")]
        public IHttpActionResult LoadUserCourse(Courses data)
        {
            var result = new Response();
            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var userCo = Convert.ToInt64(userWithClaims.Claims.First(c => c.Type == "userId").Value);

                var res = ClsDatabase.ExecuteDatatableSP("SpCourseByUserLoad",
                ClsDatabase.GenParameters("@UserCo", userCo), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("loadStates")]
        public IHttpActionResult LoadStates(State data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPGEOLoadState",
                ClsDatabase.GenParameters("@StateCo", data.StateId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("loadCity")]
        public IHttpActionResult LoadCity(State data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPGEOLoadCity",
                ClsDatabase.GenParameters("@StateCo", data.StateId,
                    "@CityCo", data.CityId), 1);

                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("loadVillage")]
        public IHttpActionResult LoadVillage(State data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPGEOLoadVillage",
                ClsDatabase.GenParameters("@CityCo", data.CityId,
                    "@VillageCo", data.VillageId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("SaveTest")]
        public IHttpActionResult SaveTest(TestModel data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCHTTestSave",
                ClsDatabase.GenParameters("@SignalCo", data.Id, "@CourseStepCo", data.CourseId, "@Question", data.Question,
                "@Answer1", data.Answer1, "@Answer2", data.Answer2, "@Answer3", data.Answer3, "@Answer4", data.Answer4, "@Answer5", data.Answer5), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("testAnswer")]
        public IHttpActionResult TestAnswer(TestAnswerModel data)
        {
            //get user Id
            var userWithClaims = (ClaimsPrincipal)User;
            var userCo = Convert.ToInt64(userWithClaims.Claims.First(c => c.Type == "userId").Value);

            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCHTTestSaveResult",
                ClsDatabase.GenParameters("@SignalCo", data.TestId, "@UserCo", userCo, "@Res", data.Answer), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("getTestResult/{id}")]
        public IHttpActionResult GetTestResult(string id)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCHTGetTestResult",
                ClsDatabase.GenParameters("@SignalCo", id), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("getStream")]
        public IHttpActionResult GetStream()
        {
            //get user Id
            var userWithClaims = (ClaimsPrincipal)User;
            var userCo = Convert.ToInt64(userWithClaims.Claims.First(c => c.Type == "userId").Value);
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPLiveStream",
                ClsDatabase.GenParameters("@UserCo", userCo), 1);

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Code = (int)ResponseType.Ok;
                result.Data = res.Rows[0].Table;

                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("loadTeacher")]
        public IHttpActionResult LoadTeacher(Teacher data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPTeacherLoad",
                ClsDatabase.GenParameters("@TeacherCo", data.TeacherId), 1);

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Code = (int)ResponseType.Ok;
                result.Data = res.Rows[0].Table;

                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("save")]
        public IHttpActionResult SaveCourse(Courses data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCourseSave",
                ClsDatabase.GenParameters("@CourseCo", data.CourseId,
                    "@Deleted", data.Deleted, "@Title", data.Title, "@Description", data.Description,
                    "@Keyword", data.Keyword, "@IsActive", data.IsActive, "@PublishDate", data.PublishDate,
                    "@Type", data.Type, "@Price", data.Price, "@IsOffered", data.IsOffered,
                    "@Index", data.Index, "@PriceRegion1", data.PriceRegion1, "@PriceRegion2",
                    data.PriceRegion2, "@PriceRegion3", data.PriceRegion3, "@StartDateTime",
                    data.StartDateTime, "@System", data.System, "@StepCount", data.StepCount,
                    "@CourseStartDescription", data.CourseStartDescription, "@TeacherCo",
                    data.TeacherId, "@StageCo", data.StageId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("saveSession")]
        public IHttpActionResult SaveStep(CourseStep data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCourseStepSave",
                ClsDatabase.GenParameters("@CourseStepCo", data.CourseStepId,
                    "@Deleted", data.Deleted, "@Title", data.Title, "@Keyword",  data.Keyword,
                    "@CourseCo", data.CourseId,"@Type", data.Type, "@IsFree", data.IsFree,
                    "@Index ", 0, "@EndDateTime", data.EndDateTime, "@StartDateTime",
                    data.StartDateTime, "@HasFile", data.HasFile, "@FileUrl",
                    data.FileUrl), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("deleteSession")]
        public IHttpActionResult DeleteSession(CourseStep data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCourseStepDelete",
                ClsDatabase.GenParameters("@CourseStepCo", data.CourseStepId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteCourse(CourseStep data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCourseDelete",
                ClsDatabase.GenParameters("@CourseCo", data.CourseId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("endSession")]
        public IHttpActionResult EndSession(CourseStep data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCourseStepEnd",
                ClsDatabase.GenParameters("@CourseStepCo", data.CourseStepId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("loadCourseAudience")]
        public IHttpActionResult LoadCourseAudience(CourseStep data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCourseAudienceLoad",
                    ClsDatabase.GenParameters("@CourseCo", data.CourseId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("saveCourseAudience")]
        public IHttpActionResult SaveCourseAudience(CourseStep data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCourseAudienceSave",
                    ClsDatabase.GenParameters("@CourseCo", data.CourseId,
                        "@AudienceCo", data.AudienceId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("deleteCourseAudience")]
        public IHttpActionResult DeleteCourseAudience(CourseStep data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPCourseAudienceDelete",
                    ClsDatabase.GenParameters("@CourseCo", data.CourseId,
                        "@AudienceCo", data.AudienceId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("loadStage")]
        public IHttpActionResult LoadStage(Courses data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPStageLoad",
                    ClsDatabase.GenParameters("@StageCo", data.StageId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("loadPackage")]
        public IHttpActionResult LoadPackage(Package data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPPackageLoad",
                    ClsDatabase.GenParameters("@PackageCo", data.PackageId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("savePackage")]
        public IHttpActionResult SavePackage(Package data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPPackageSave",
                    ClsDatabase.GenParameters("@PackageCo", data.PackageId,
                        "@Deleted", data.Deleted, "@Title", data.Title, "@Description",
                        data.Description, "@StartDateTime", data.StartDateTime, "@IsActive",
                        data.IsActive, "@PublishDate", data.PublishDate, "@DiscountPercentage",
                        data.DiscountPercentage, "@IsOffered", data.IsOffered, "@Index", data.Index,
                        "@StageCo", data.StageId, "@Type", data.Type, "@System", data.System,
                        "@AudienceCo", data.AudienceId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("deletePackage")]
        public IHttpActionResult DeletePackage(Package data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPPackageDelete",
                    ClsDatabase.GenParameters("@PackageCo", data.PackageId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("savePackageCourse")]
        public IHttpActionResult SavePackageCourse(Package data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPPackageCourseSave",
                    ClsDatabase.GenParameters("@PackageCo", data.PackageId,
                        "@CourseCo", data.CourseId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("loadPackageCourse")]
        public IHttpActionResult LoadPackageCourse(Package data)
        {
            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPPackageCourseLoad",
                    ClsDatabase.GenParameters("@PackageCo", data.PackageId), 1);
                result.Code = (int)ResponseType.Ok;

                if (result.Code == 200)
                {
                    result.Message = "Success";
                }
                result.Data = res.Rows[0].Table;


                return Ok(result);
            }
            catch (Exception e)
            {
                string ex = e.InnerException.ToString();
                result.Code = 400;
                result.Message = "Server error";
                return Ok(result);
            }
        }

    }
}
