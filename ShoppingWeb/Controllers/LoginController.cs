using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using ShoppingWeb.Models.ViewModels;
using ShoppingWeb.Models.Identity;
using Microsoft.Owin.Security;
using System.Web.Http.Cors;
using ShoppingWeb.Constants;
using ShoppingWeb.Interface;
using ShoppingWeb.DbOps;
using System.Text;

namespace ShoppingWeb.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : Controller
    {
        private readonly IdentityOperations _idOps;
        public LoginController()
        {
            _idOps = new IdentityOperations();
        }
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult Login([System.Web.Http.FromBody]LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;
                var userName = string.IsNullOrEmpty(login.LastName) ? login.FirstName : login.Email;
                SyncIdentityUser user = new SyncIdentityUser();
                var error = "";
                user = userManager.Find(userName, login.Password);
                try
                {
                    if (user != null)
                    {
                        _idOps.SignInUser(user);
                    }
                    else
                    {
                        var PasswordHash = new PasswordHasher();

                        var newUser = new SyncIdentityUser
                        {
                            UserName = userName,
                            Email = login.Email,
                            PasswordHash = PasswordHash.HashPassword(login.Password)
                        };

                        user = _idOps.CreateUser(newUser);
                        _idOps.SignInUser(user);
                    }
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
                return Json(error);
                //return Json(login);
            }
            ModelState.AddModelError("", "Invalid username or password");
            return Json(false);
        }

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult CheckLogin()
        {
            var loggedIn = Request.IsAuthenticated;
            return Json(loggedIn);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}