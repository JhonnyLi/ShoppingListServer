using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ShoppingWeb.Hubs
{
    public class SyncHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name,message);
        }
    }
}