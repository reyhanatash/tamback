using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static Tamland_API.Models.User;
using SmsIrRestful;


namespace Tamland_API.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        enum ResponseType
        {
            Ok = 200,
            NotOk = 400
        }

        [HttpPost]
        [Route("signup")]
        public DataTable SignUp(Register data)
        {
            var res = ClsDatabase.ExecuteDatatableSP("SPUserSignUp",
                ClsDatabase.GenParameters("@Mobile", data.Mobile), 1);
            if (Convert.ToInt32(res.Rows[0][1]) == 0)
            {
                Sendsms(data.Mobile, "کد ورود : " + res.Rows[0][2] + Environment.NewLine + "تاملند مدرسه ای برای همه ",
                    1, "", "");
            }

            return res;
        }

        [HttpPost]
        [Route("verifyCode")]
        public DataTable VerifyCode(Register data)
        {
            var res = ClsDatabase.ExecuteDatatableSP("SPUserVerifyCode",
                ClsDatabase.GenParameters("@Mobile", data.Mobile, "@VerifyCode",
                    data.VerificationCode), 1);
            return res;
        }

        [HttpPost]
        [Route("changePassword")]
        public DataTable ChangePassword(ForgotPassword data)
        {
            var res = ClsDatabase.ExecuteDatatableSP("SPUserChangePassword",
                ClsDatabase.GenParameters("@Mobile", data.Mobile, "@OldPassWord",
                    data.OldPassword, "@NewPassWord", data.NewPassword), 1);
            return res;
        }

        [HttpPost]
        [Route("forgotPassword")]
        public DataTable ForgotPassword(Register data)
        {
            var res = ClsDatabase.ExecuteDatatableSP("SPUserForgetPassword",
                ClsDatabase.GenParameters("@Mobile", data.Mobile), 1);
            if (Convert.ToInt32(res.Rows[0][1]) == 0)
            {
                Sendsms(data.Mobile, "کد ورود : " + res.Rows[0][2] + Environment.NewLine + "تاملند مدرسه ای برای همه ",
                    1, "", "");
            }

            return res;
        }


        [HttpPost]
        [Route("resetPassword")]
        public DataTable ResetPassword(ForgotPassword data)
        {
            var res = ClsDatabase.ExecuteDatatableSP("SPUserChangePasswordFirst",
                ClsDatabase.GenParameters("@Mobile", data.Mobile , "@NewPassWord", data.NewPassword), 1);
            return res;
        }


        [HttpPost]
        [Authorize]
        [Route("saveUserArea")]
        public IHttpActionResult SaveUserArea(Area data)
        {

            var result = new Response();
            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var userCo = Convert.ToInt64(userWithClaims.Claims.First(c => c.Type == "userId").Value);

                var res = ClsDatabase.ExecuteDatatableSP("SPUserSetVillageAndRegion",
                ClsDatabase.GenParameters("@UserCo", userCo,
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

        [HttpGet]
        [Authorize]
        [Route("loadInfo")]
        public IHttpActionResult LoadInfo()
        {

            var result = new Response();
            try
            {
                var userWithClaims = (ClaimsPrincipal)User;
                var userCo = Convert.ToInt64(userWithClaims.Claims.First(c => c.Type == "userId").Value);
                // var userCo = 21262;

                var res = ClsDatabase.ExecuteDatatableSP("SPUserLoad",
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
        [Route("loadAudience")]
        public IHttpActionResult LoadAudience(Info data)
        {

            var result = new Response();
            try
            {
                var res = ClsDatabase.ExecuteDatatableSP("SPAudienceLoad",
                ClsDatabase.GenParameters("@AudienceCo", data.AudienceId), 1);
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
        [Route("saveInfo")]
        public IHttpActionResult SaveInfo(Info data)
        {

            var result = new Response();
            try
            {
                //string cardfilename = GetHashString(data.CardFileName);
                // string cfilename = cardfilename;

                if (!string.IsNullOrEmpty(data.CardImage))
                {
                    byte[] cardbytes = Convert.FromBase64String(data.CardImage);
                    Image image;
                    using (MemoryStream ms = new MemoryStream(cardbytes))
                    {
                        image = Image.FromStream(ms);
                    }


                    var edbytess = Convert.FromBase64String(data.CardImage);
                    using (var imageFile =
                        new FileStream(HttpContext.Current.Server.MapPath("~/Files/card/" + data.CardFileName),
                            FileMode.Create))
                    {
                        imageFile.Write(cardbytes, 0, edbytess.Length);
                        imageFile.Flush();
                    }
                }

                // string myfilename = GetHashString(data.EduFileName);
                // string filename = myfilename;

                if (!string.IsNullOrEmpty(data.EduImage))
                {
                    byte[] edubytes = Convert.FromBase64String(data.EduImage);
                    Image image2;
                    using (MemoryStream ms = new MemoryStream(edubytes))
                    {
                        image2 = Image.FromStream(ms);
                    }


                    var edubytess = Convert.FromBase64String(data.EduImage);
                    using (var imageFile =
                        new FileStream(HttpContext.Current.Server.MapPath("~/Files/edu/" + data.EduFileName),
                            FileMode.Create))
                    {
                        imageFile.Write(edubytes, 0, edubytess.Length);
                        imageFile.Flush();
                    }
                }

                var userWithClaims = (ClaimsPrincipal)User;
                var userCo = Convert.ToInt64(userWithClaims.Claims.First(c => c.Type == "userId").Value);
                // var userCo = 21262;
                var res = ClsDatabase.ExecuteDatatableSP("SPUserProfileChange",
                ClsDatabase.GenParameters("@UserCo", userCo, "@Email",
                    data.Email, "@FullName", data.FullName, "@Gender", data.Gender,
                    "@BD", data.BirthDate, "@AudienceCo", data.AudienceId, "IDCardPicAddress",
                    data.CardFileName, "@CerificetePicAddress", data.EduFileName), 1);
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
                result.Message = ex;
                return Ok(result);
            }
        }


        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        [HttpGet]
        [Route("send")]
        public bool Sendsms(string phoneNumber, string message, int cat, string fname, string lname)
        {
            var result = new Response();
            try
            {
                SmsIrRestful.Token tokenInstance = new SmsIrRestful.Token();
                var token = tokenInstance.GetToken("ca3434a7ac76118b40a34a7", "tVuqAmCHGb@Dwy3m#cXmMm");
                int category = 49512;
                if (cat == 1) // tamland
                    category = 49512;

                SmsIrRestful.CustomerClubContact contact = new SmsIrRestful.CustomerClubContact();
                var s = contact.Create(token, new CustomerClubContactObject()
                {
                    Mobile = phoneNumber,
                    CategoryId = category,
                    FirstName = fname,
                    LastName = lname
                });

                var customerClubSend = new CustomerClubSend()
                {
                    Messages = new List<string>() { message }.ToArray(),
                    MobileNumbers = new List<string>() { phoneNumber }.ToArray(),
                    SendDateTime = null,
                    CanContinueInCaseOfError = false
                };

                var customerClubContactResponse = new CustomerClub().Send(token, customerClubSend);
                //                result.Code = (int)ResponseType.Ok;
                //                result.Message = customerClubContactResponse.Message;
                return customerClubContactResponse.IsSuccessful;
                //                return Ok(result);
            }
            catch (Exception e)
            {
                var result2 = new Response();
                result2.Code = 400;
                result2.Message = e.Message;
                return false;

            }
        }

    }
}
