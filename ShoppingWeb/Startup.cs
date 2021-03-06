﻿using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

//[assembly: OwinStartup("SignalR",typeof(ShoppingWeb.Startup))]
namespace ShoppingWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            var hubConfiguration = new HubConfiguration();
            //#if DEBUG

            hubConfiguration.EnableDetailedErrors = true;
            //#endif
            try
            {
                app.MapSignalR(hubConfiguration);
            }
            catch (Exception e)
            {
                var fail = e;
            }
        }
    }
}
