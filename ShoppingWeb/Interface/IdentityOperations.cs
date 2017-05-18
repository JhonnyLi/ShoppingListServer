using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
using ShoppingWeb.Models.Identity;
using System.Web;
using Microsoft.AspNet.Identity;
using ShoppingWeb.DbOps;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ShoppingWeb.Constants;

namespace ShoppingWeb.Interface
{
    public class IdentityOperations : IIdentityOperations
    {
        private readonly DatabaseContext _ctx;
        private readonly RoleManager<SyncIdentityRole> _roleManager;
        private readonly UserManager<SyncIdentityUser> _userManager;

        public IdentityOperations()
        {
            _ctx = new DatabaseContext();
            _roleManager = new RoleManager<SyncIdentityRole>(new RoleStore<SyncIdentityRole>(_ctx));
            _userManager = new UserManager<SyncIdentityUser>(new UserStore<SyncIdentityUser>(_ctx));
            var authManager = HttpContext.Current.GetOwinContext().Authentication;
        }
        public void SignInUser(SyncIdentityUser user)
        {
            var authManager = HttpContext.Current.GetOwinContext().Authentication;
            var ident = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
        }

        public void SignOutUser(SyncIdentityUser user)
        {
            throw new NotImplementedException();
        }
        public List<SyncIdentityRole> GetAllRoles()
        {
            //throw new NotImplementedException();
            return _roleManager.Roles.ToList();
        }

        /// <summary>
        /// Returns the user if found, else it returns null
        /// </summary>
        /// <param name="name"></param>
        /// <returns>SyncIdentityUser | null</returns>
        public SyncIdentityUser GetUser(string name)
        {
            //throw new NotImplementedException();
            var userExists = _userManager.FindByName(name);
            if (userExists != null)
            {
                return userExists;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Check if a specific name already exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns true or false</returns>

        public bool UserNameExists(string name)
        {
            return _userManager.FindByName(name) != null ? true : false;
        }
        /// <summary>
        /// Check if a specific email already exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns true or false</returns>
        public bool UserEmailExists(string email)
        {
            return _userManager.FindByEmail(email) != null ? true : false;
        }

        public SyncIdentityUser CreateUser(SyncIdentityUser user)
        {
            _userManager.Create(user);
            _userManager.AddToRole(user.Id, SyncConstants.User);
            return user;
        }
    }
}