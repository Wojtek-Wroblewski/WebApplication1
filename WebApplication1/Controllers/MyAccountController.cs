using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2;
using WebApplication2.Models;
using static WebApplication2.DBConnect.Register;

namespace WebApplication2.Controllers
{
    public class MyAccountController : Controller
    {
        public object UserAccount { get; private set; }

        // GET: Accaount
        public ActionResult Index()
        {
            return View();
        }

        public string Login (string userName, string PassWord)
        {

            UserAccount z = new UserAccount();
            var model = z.ReturnList().Where(x => x.UserName == userName && x.Password == PassWord).SingleOrDefault();
            if (model!=null)
            { 
            //Create cookies
            HttpCookie userCookie = new HttpCookie("user", model.UserID.ToString());

            //Expire Date
            userCookie.Expires.AddDays(10);

            //Save data at cookies
            HttpContext.Response.SetCookie(userCookie);
}
            //Get user data from cookie
            HttpCookie NewCookie = Request.Cookies["user"];

            return NewCookie.Value;

        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            
            if (ModelState.IsValid)
            {
                /*
                 TODO
                 if (account.EmailWolny)

                 */
                var Registered = NewAccount(account);
                if (Registered != null)
                {

                    //Create cookies
                    HttpCookie userCookie = new HttpCookie("Hash|Salt", Registered.hash.ToString() + "|" + Registered.salt.ToString());

                    //Expire Date
                    userCookie.Expires.AddDays(10);

                    //Save data at cookies
                    HttpContext.Response.SetCookie(userCookie);


                    return RedirectToAction("Index", "MyAccountController");
                }
                else
                {
                    ModelState.AddModelError("Email", "An account already exists for this email address.");
                }

            }

            return View();
        }

        //public ActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Login(UserAccount user)
        //{

        //    return View();
        //}


    }
} 