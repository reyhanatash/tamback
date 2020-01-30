using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tamland_API.Models
{
    public class User
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

        public class Register
        {
            public string Mobile { get; set; }
            public string VerificationCode { get; set; }
        }

        public class ForgotPassword
        {
            public string Mobile { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        public class Area
        {
            public int UserId { get; set; }
            public long VillageId { get; set; }
        }

        public class Info
        {
            public string Email { get; set; }
            public string FullName { get; set; }
            public int Gender { get; set; }
            public int AudienceId { get; set; }
            public DateTime BirthDate { get; set; }
            public string CardFileName { get; set; }
            public string CardImage { get; set; }
            public string EduFileName { get; set; }
            public string EduImage { get; set; }
        }



    }
}