using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using ShoppingWeb.App_Code;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Hosting;
using ShoppingWeb.Interface;
using ShoppingWeb.Models;
using ShoppingWeb.Models.ViewModels;
using ShoppingWeb.Models.Identity;

namespace ShoppingWeb.Hubs
{
    public class SyncHub : Hub
    {
        private readonly DbOperations _ctx;
        private readonly IdentityOperations _idOps;
        //private readonly SyncIdentityUser _user;
        public SyncHub()
        {
            _ctx = new DbOperations();
            _idOps = new IdentityOperations();
            //_user = _idOps.GetUser(Context.QueryString["username"]);
        }
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
            if (!GlobalStorage.Clients.Any(n => n.Value == name))
            {
                GlobalStorage.Clients.Add(callerId, name);
            }
            //Users skall skickas till klienterna och uppdatera vilka som är online.
            //Hela listan skickas för att alla ska ha samma lista.
            var jsonList = GetShoppingListViewModel();
            Clients.All.listMessage("Server", jsonList);
            var users = JsonConvert.SerializeObject(GlobalStorage.Clients.Select(n=>n.Value).ToList());
            Clients.All.usersOnlineMessage(users);
            //Clients.All.connectionMessage(name, $"{name} connected");
        }

        public void SendList(string updatedList)
        {
            var list = JsonConvert.DeserializeObject<ShoppingListViewModel>(updatedList);
            _ctx.AddOrUpdateList(list);
            string userName = Context.QueryString["username"];
            var jsonList = GetShoppingListViewModel();
            //var shoppinglist = _ctx.GetAllShoppingLists().First();
            //var jsonList = JsonConvert.SerializeObject(shoppinglist);
            Clients.All.listMessage(userName, jsonList);
            //Clients.All.listUpdateMessage("Server says", jsonList);
        }
        //public async Task<bool> SendList(string updatedList)
        //{
        //    var list = JsonConvert.DeserializeObject<ShoppingListViewModel>(updatedList);
        //    await _ctx.AddOrUpdateListAsync(list);
        //    string userName = Context.QueryString["username"];
        //    var jsonList = GetShoppingListViewModel();
        //    //var shoppinglist = _ctx.GetAllShoppingLists().First();
        //    //var jsonList = JsonConvert.SerializeObject(shoppinglist);
        //    await Clients.All.listUpdateMessage(userName, jsonList);
        //    return true;
        //    //Clients.All.listUpdateMessage("Server says", jsonList);
        //}
        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<SyncHub>();
            //var serial = JsonConvert.SerializeObject(Context.QueryString);
            //Clients.All.contextMessage(serial);
            //hubContext.Clients.All.contextMessage(serial);
            var name = Context.QueryString["username"];
            //hubContext.Clients.All.connectionMessage(name, "Woop");
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

        private string GetShoppingListViewModel()
        {
            var shoppinglist = _ctx.GetAllShoppingLists().First();
            var listToSend = new ShoppingListViewModel()
            {
                ShoppingListId = shoppinglist.ShoppingListId,
                Name = shoppinglist.Name,
                Items = new List<ItemViewModel>(),
                ListUpdated = false
            };
            foreach (var item in shoppinglist.Items)
            {
                var viewModelItem = new ItemViewModel()
                {
                    ItemId = item.ItemId.ToString(),
                    Name = item.Name,
                    Active = item.Active,
                    Comment = item.Comment,
                    Deleted = false
                };
                listToSend.Items.Add(viewModelItem);
            }
            //var jsonList = JsonConvert.SerializeObject(shoppinglist);
            var jsonList = JsonConvert.SerializeObject(listToSend);
            return jsonList;
        }

        #region Statics for server use
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
        public static void SendListUpdate()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SyncHub>();
            //listUpdateMessage måste läggas till i alla klienter så att dom får en uppdaterad lista.
            //var list = JsonConvert.SerializeObject(GlobalStorage.Clients.ToList());
            var _db = new DbOperations();
            List<Item> list = _db.GetAllShoppingLists().First().Items;
            hubContext.Clients.All.listMessage("Server", list);
        }
        #endregion
    }
}