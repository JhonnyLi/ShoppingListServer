using ShoppingWeb.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ShoppingWeb.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize]
        public ActionResult Index()
        {
            ViewData["Title"] = "Start";
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<SyncHub>();
            //hubContext.Clients.All.broadcastMessage("Server","Testmeddelande från servern");
            //_signalRClient.Send("Server", "Test");
            //SyncHub.SendMessage("Server ","Testmeddelande från servern");
            return View();
        }
        public ActionResult Login()
        {
            ViewData["Title"] = "Login";
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<SyncHub>();
            //hubContext.Clients.All.broadcastMessage("Server","Testmeddelande från servern");
            //_signalRClient.Send("Server", "Test");
            //SyncHub.SendMessage("Server ","Testmeddelande från servern");
            return View();
        }
    }
}
