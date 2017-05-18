using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using ShoppingWeb.DbOps;
using ShoppingWeb.Models.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

[assembly: OwinStartup("Identity", typeof(ShoppingWeb.App_Start.IdentityConfig))]
namespace ShoppingWeb.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new DatabaseContext());
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<RoleManager<SyncIdentityRole>>((options, context) =>
                new RoleManager<SyncIdentityRole>(
                    new RoleStore<SyncIdentityRole>(context.Get<DatabaseContext>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Index"),
            });

            var hubConfiguration = new HubConfiguration();
#if DEBUG

            hubConfiguration.EnableDetailedErrors = true;
#endif
            try
            {
                app.MapSignalR(hubConfiguration);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

    }
}