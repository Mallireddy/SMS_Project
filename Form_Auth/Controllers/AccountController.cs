using Form_Auth.Models;
using Form_Auth.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Form_Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly Form_Auth_Entities context = new Form_Auth_Entities();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(LoginModel model)
        {
            using(var dbContext = new Form_Auth_Entities())
            {
                if (ModelState.IsValid)
                {
                    var result = dbContext.Users.Any(e => e.Username == model.UserName.ToLower() && e.Password == model.Password.ToLower());
                    if(result !=false)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        return RedirectToAction("EmployeeList", "Employee");
                    }
                }
               
            }
            ViewBag.ModelState = "Invalid username & Password";
            return View();
        }
        public ActionResult LogoutUser()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(User model)
        {
            context.Users.Add(model);
            context.SaveChanges();
            return View();
        }



    }
}