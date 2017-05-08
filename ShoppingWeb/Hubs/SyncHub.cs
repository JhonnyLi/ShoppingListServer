using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using ShoppingWeb.App_Code;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Hosting;

namespace ShoppingWeb.Hubs
{
    public class SyncHub : Hub
    {
        
        public void Send(string name, string message)
        {
            //string userName = Clients.Caller.userName;
            string userName = Context.QueryString["username"];
            Clients.All.broadcastMessage(userName, message);
        }
        public void ClientConnected()
        {
            var callerId = Context.ConnectionId;
            var name = Context.QueryString["username"];
            var serial = JsonConvert.SerializeObject(Context.QueryString);
            Clients.All.contextMessage(serial);
            if (!GlobalStorage.Clients.Any(n=>n.Value == name))
            {
                GlobalStorage.Clients.Add(callerId, name);
            }
            Clients.All.connectionMessage(name, $"{name} connected");
            //Clients.All.contextMessage(serial);
        }
        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SyncHub>();
            var serial = JsonConvert.SerializeObject(Context.QueryString);
            Clients.All.contextMessage(serial);
            hubContext.Clients.All.contextMessage(serial);
            //var callerId = Context.ConnectionId;
            //var info = Context.User;
            //var caller = Clients.Caller;
            //string userName = Clients.Caller.userName;
            //var users = hubContext.Clients.All;
            var name = Context.QueryString["username"];
            hubContext.Clients.All.connectionMessage(name, "Woop");
            SyncHub.SendMessage("Server", $"{name} connected");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            SyncHub.SendMessage("Server", "Client disconnected");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            // Add your own code here.
            // For example: in a chat application, you might have marked the
            // user as offline after a period of inactivity; in that case 
            // mark the user as online again.
            return base.OnReconnected();
        }
        public static void SendMessage(string name, string msg)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SyncHub>();
            hubContext.Clients.All.broadcastMessage(name, msg);
        }
        public static void SendContext(string serial)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SyncHub>();
            hubContext.Clients.All.contextMessage("Server", serial);
        }
    }
}