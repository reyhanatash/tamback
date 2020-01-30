using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Tamland_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:7080");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:600");

            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,PUT,POST,DELETE,PATCH,OPTIONS");

                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, Accept, Content-Type, Authorization, Access-Control-Allow-Origin, x-requested-with, grant_type");
                HttpContext.Current.Response.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                HttpContext.Current.Response.AddHeader("Accept", "application/json, text/plain, */*");
                HttpContext.Current.Response.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }
    }
}
