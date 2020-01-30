using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using pisalon;
[assembly: OwinStartup(typeof(Tamland_API.Startup))]

namespace Tamland_API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
                {
                    Provider = new QueryStringOAuthBearerProvider()
                });

                var hubConfiguration = new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    Resolver = GlobalHost.DependencyResolver,
                };
                map.RunSignalR(hubConfiguration);
            });
            //            var hubConfiguration = new HubConfiguration();
            //            hubConfiguration.EnableDetailedErrors = true;
            //
            //            app.MapSignalR(hubConfiguration);
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            //enable cors origin requests
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var myProvider = new MyAuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
                Provider = myProvider

            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }



    }

    public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get("access_token");

            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }

            return Task.FromResult<object>(null);
        }
    }
}
