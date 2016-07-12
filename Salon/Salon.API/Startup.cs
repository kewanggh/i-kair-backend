using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Salon.API.Infrastructure;
using Stripe;
using System.Configuration;
using Salon.API.App_Start;

[assembly: OwinStartup(typeof(Salon.API.Startup))]
namespace Salon.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            ConfigureOAuth(app);
            //
            //app.CreatePerOwinContext<SalonUserManager>(SalonUserManager.Create);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);

            config.Formatters.JsonFormatter
                        .SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeSecretKey"]);
            HangFire.ConfigureHangfire(app);
            HangFire.InitializeJobs();
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            // configure authentication
            var authenticationOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(authenticationOptions);

            // Configure authorization
            var authorizationOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SalonAuthorizationServerProvider()
            };
            app.UseOAuthAuthorizationServer(authorizationOptions);

        }

    }
}