using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWeb.App_Code
{
    public static class GlobalStorage
    {
        static Dictionary<string, string> _clients = new Dictionary<string, string>();

        public static Dictionary<string, string> Clients
        {
            get { return _clients; }
            set { _clients = value; }
        }
    }
}