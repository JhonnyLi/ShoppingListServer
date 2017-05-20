using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWeb.Models.Identity
{
    public class SyncIdentityRole : IdentityRole
    {
        public SyncIdentityRole() : base() { }
        public SyncIdentityRole(string name) : base(name) { }

    }
}