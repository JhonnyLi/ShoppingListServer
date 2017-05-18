using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ShoppingWeb.Models;
using ShoppingWeb.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;

namespace ShoppingWeb.DbOps
{
    public class DatabaseContext : IdentityDbContext<SyncIdentityUser>
    {
        public DatabaseContext() : base("name=SyncDb_debug")
        {

        }

        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Item> Items { get; set; }
        
    }
    public class AppUserManager : UserManager<SyncIdentityUser>
    {
        public AppUserManager(IUserStore<SyncIdentityUser> store)
            : base(store)
        {
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(
                new UserStore<SyncIdentityUser>(context.Get<DatabaseContext>()));
            // optionally configure your manager
            // ...

            return manager;
        }
    }
    public class AppUserManager : UserManager<SyncIdentityUser>
    {
        public AppUserManager(IUserStore<SyncIdentityUser> store)
            : base(store)
        {
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(
                new UserStore<SyncIdentityUser>(context.Get<DatabaseContext>()));
            // optionally configure your manager
            // ...

            return manager;
        }
    }
}