using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingWeb.Models;
using ShoppingWeb.DbOps;
using ShoppingWeb.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoppingWeb.Constants;

namespace ShoppingWeb.Interface
{
    public class DbOperations : IDbOperations
    {
        //Database access
        private readonly DatabaseContext _ctx;
        //Identity managers.
        private readonly RoleManager<SyncIdentityRole> _roleManager;
        private readonly UserManager<SyncIdentityUser> _userManager;
        public DbOperations()
        {
            _ctx = new DatabaseContext();
            _roleManager = new RoleManager<SyncIdentityRole>(new RoleStore<SyncIdentityRole>(_ctx));
            _userManager = new UserManager<SyncIdentityUser>(new UserStore<SyncIdentityUser>(_ctx));
        }

        public bool AddNewList(ShoppingList model)
        {
            bool success = true;
            UpdateItems(model.Items);
            try
            {
                _ctx.ShoppingLists.Add(model);
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                success = false;
                var message = ex.Message;
                throw ex;
            }
            return success;

        }

        private List<Item> UpdateItems(List<Item> items)
        {
            foreach(var item in items)
            {
                item.ItemId = Guid.NewGuid();
            }
            return items;
        }

        public List<Item> GetAllItems()
        {
            var model = _ctx.Items.OrderBy(n=>n.Name).ToList();
            return model;
        }

        public List<ShoppingList> GetAllShoppingLists()
        {
            var model = _ctx.ShoppingLists.OrderBy(n=>n.Name).ToList();
            return model;
        }

        public ShoppingList GetShoppingListByIndex(int index)
        {
            var model = _ctx.ShoppingLists.OrderBy(n => n.Name).ToList()[index];
            return model;
        }

        public bool RemoveList(Guid model)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get a list of all availible roles.
        /// </summary>
        /// <returns>List<SyncIdentityRole></returns>
        //public List<SyncIdentityRole> GetAllRoles()
        //{
        //    //throw new NotImplementedException();
        //    return _roleManager.Roles.ToList();
        //}

        ///// <summary>
        ///// Returns the user if found, else it returns null
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns>SyncIdentityUser | null</returns>
        //public SyncIdentityUser GetUser(string name)
        //{
        //    //throw new NotImplementedException();
        //    var userExists = _userManager.FindByName(name);
        //    if(userExists != null)
        //    {
        //        return userExists;
        //    }else
        //    {
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// Check if a specific name already exists.
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns>Returns true or false</returns>

        //public bool UserNameExists(string name)
        //{
        //    return _userManager.FindByName(name) != null ? true : false;
        //}
        ///// <summary>
        ///// Check if a specific email already exists.
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns>Returns true or false</returns>
        //public bool UserEmailExists(string email)
        //{
        //    return _userManager.FindByEmail(email) != null ? true : false;
        //}

        //public SyncIdentityUser CreateUser(SyncIdentityUser user)
        //{
        //    _userManager.Create(user);
        //    _userManager.AddToRole(user.Id, SyncConstants.User);
        //    return user;
        //}
    }
}