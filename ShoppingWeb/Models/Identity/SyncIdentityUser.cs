using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWeb.Models.Identity
{
    public class SyncIdentityUser : IdentityUser
    {
        public string FacebookToken { get; set; }
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }

    }
}