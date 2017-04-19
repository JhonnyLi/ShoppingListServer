using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ShoppingWeb.Models;

namespace ShoppingWeb.DbOps
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=SyncDb")
        {

        }

        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Item> Items { get; set; }

    }
}