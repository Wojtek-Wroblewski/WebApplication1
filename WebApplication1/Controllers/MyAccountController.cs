using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2;
using WebApplication2.Models;
using static WebApplication2.DBConnect.Register;
using static WebApplication2.DBConnect.Logging;
using System.Diagnostics;

namespace WebApplication2.Controllers
{
    public class MyAccountController : Controller
    {
        public object UserAccount { get; private set; }

        // GET: Accaount
        public ActionResult Index()
        {
            HttpCookie Logged = Request.Cookies["Logged"];
            HttpCookie RememberMe = Request.Cookies["RememberMe"];
            RegisterAccount Registered = (RegisterAccount)TempData["Registered"];
            LoginAccount login = (LoginAccount)TempData["Login"];

                if (Logged != null)
            {
                Debug.WriteLine(Logged.Value);
            }
            if (RememberMe != null)
            {
                Debug.WriteLine(RememberMe.Value);
            }

            if (RememberMe != null)
            {
               /* if(IsSessionInDB(NewCookie))*/
                /*Strona dla zalogowanych*/
            }
            else
            { /*Dla NIEzalogowanych */}

            return View();

        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginAccount model)
        {

            LoginAccount z = new LoginAccount();
            //var model = z.ReturnList().Where(x => x.UserName == userName && x.Password == PassWord).SingleOrDefault();
            if (ModelState.IsValid)
            if (model.Email != null && model.Password!=null)
                {

                   switch(Logg(model))
                    {
                        case "Success|True":
                            {
                                TempData["Login"] = model;
                                return RedirectToAction("Index", "MyAccount");
                                break;
                            }
                        case "Success|False":
                            {

                                TempData["Login"] = model;
                                return RedirectToAction("Index", "MyAccount");
                                break;
                            }
                        case "Password":
                            {

                                ModelState.AddModelError("Password", "Wrong password");
                                break;
                            }
                        case "Email":
                            {

                                ModelState.AddModelError("Email", "Wrong Email");
                                break;
                            }
                    }




                    //Create cookies
                    HttpCookie userCookie = new HttpCookie("user", model.UserID.ToString());

            //Expire Date
            userCookie.Expires.AddDays(10);

            //Save data at cookies
            HttpContext.Response.SetCookie(userCookie);
                    
}
            //Get user data from cookie
            HttpCookie NewCookie = Request.Cookies["user"];

            //return NewCookie.Value;
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterAccount account)
        {
            
            if (ModelState.IsValid)
            {
                /*
                 TODO
                 if (account.EmailWolny)

                 */
                if (ModelState.ContainsKey("Email"))
                    ModelState["Email"].Errors.Clear();
                var Registered = NewAccount(account);

                if (Registered != null)
                {
                    HttpCookie userCookie = new HttpCookie("Session");
                    if (account.RememberMe)
                    {
                    //Create cookies
                    //HttpCookie userCookie = new HttpCookie("Session", Registered.Session);
                    userCookie.Value = Registered.Session;

                    //Expire Date
                    userCookie.Expires.AddDays(10);

                    //Save data at cookies
                    HttpContext.Response.SetCookie(userCookie);
                    }
                    else
                    {
                        userCookie.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Response.SetCookie(userCookie);
                    }
                    
                    
                   LoginAccount login = new LoginAccount
                    {
                        Email = account.Email,
                        Password = account.Password,
                        Session = account.Session,
                        Hash = account.Hash,
                        Salt = account.Salt,
                        RememberMe = account.RememberMe,
                        UserID = account.UserID
                    };

                    TempData["Login"] = account;

                    return RedirectToAction("Index", "MyAccount");
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
        //public ActionResult Login(RegisterAccount user)
        //{

        //    return View();
        //}


    }
} 