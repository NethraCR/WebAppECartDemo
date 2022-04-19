using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebAppECartDemo.Models;

namespace WebAppECartDemo.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            using (ECartDBEntities entities = new ECartDBEntities())
            {
                bool IsValidUser = entities.Users.Any(u => u.UserName.ToLower() == model.UserName.ToLower() &&
                u.UserPassword == model.UserPassword);
                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Shopping");
                }
                ModelState.AddModelError("", "Invalid User Name or Password");
                return View();
            }
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User model)
        {
            using (ECartDBEntities entities = new ECartDBEntities())
            {
                entities.Users.Add(model);
                entities.SaveChanges();
            }
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}