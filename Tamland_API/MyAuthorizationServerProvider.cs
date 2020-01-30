using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Tamland_API;
//using static Tamland_API.Models.reserve;

namespace pisalon
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        static string UserRes;
        static string UserRole;
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); // 
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var form = await context.Request.ReadFormAsync();
            var res = ClsDatabase.ExecuteDatatableSP("SPUserLogin",
                ClsDatabase.GenParameters("@Mobile", context.UserName,
                    "@PassWord", context.Password), 1);
            if (res.Rows[0]["message"].ToString() == "wrong")
            {
                var err = context.Error;
                context.SetError("invalid_grant", "Invalid username or password");
                return;
            }
            if (res.Rows[0]["message"].ToString() == "Succeed")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("userId", res.Rows[0]["UserCode"].ToString()));
                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                string secret = "";
                if (res.Rows[0]["UserRole"].ToString() == "5")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Member"));
                    secret = "4842591949";
                }
                else if (res.Rows[0]["UserRole"].ToString() == "3")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    secret = "8592737698";
                }
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                     "secret", secret
                    }
                });
                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            //if (context.UserName == "1")
            //{
            //    var claims = new List<Claim>();
            //    claims.Add(new Claim("userId", "1"));
            //    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            //    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            //    context.Validated(identity);
            //}
            //else
            //{
            //    var claims = new List<Claim>();
            //    claims.Add(new Claim("userId", "2"));
            //    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            //    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "Member"));
            //    context.Validated(identity);
            //}
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}