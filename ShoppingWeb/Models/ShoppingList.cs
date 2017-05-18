using ShoppingWeb.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingWeb.Models
{
    public class ShoppingList
    {
        public Guid ShoppingListId { get; set; }
        public string Name { get; set; }
        public virtual SyncIdentityUser User { get; set; }
        public virtual List<SyncIdentityUser> Users { get; set; } = new List<SyncIdentityUser>();
        public virtual List<Item> Items { get; set; } = new List<Item>();
    }
}