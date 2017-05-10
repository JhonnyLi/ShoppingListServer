using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ShoppingWeb.DbOps;
using Microsoft.AspNet.Identity;
using ShoppingWeb.Models.ViewModels;
using ShoppingWeb.Models.Identity;
using Microsoft.Owin.Security;
using System.Web.Http.Cors;

namespace ShoppingWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login([System.Web.Http.FromBody]LoginViewModel login)
        {
            if (ModelState.IsValid && login.Password == login.ConfirmPassword)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;

                SyncIdentityUser user = userManager.Find(login.UserName, login.Password);
                if (user != null)
                {
                    var ident = userManager.CreateIdentity(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    authManager.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
                    //return Redirect(Url.Action("Index", "Home"));
                    return Json(login);
                }
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
        public ActionResult LogOff()
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