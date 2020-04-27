using DataLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models; 
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Security;
using System.Web.Mvc;


namespace WebApplication2.DBConnect
{
    public static class Logging
    {

        public static string  Loginto (string Email, string Password)
        {
            string sql = @"SELECT * from dbo.Users where Email = '" + Email + "';";
            var data = SqlDataAccess.LoadData<RegisterAccount>(sql);
            if (data.Count != 0)
            {
                data.First().Password = Password;
                if (CorrectPassword(data.First()))
                {
                    return "Success";
                }
                return "Password";
            }
            else
                return "Email"; 

        }

        public static RegisterAccount Logout (RegisterAccount model)
        {

                return null;
        }

        
        public static bool EmailFree(string Email)
        {/*@"select * from dbo.Artysci where ArtId='" + Id+"';";// where Nazwa1='@_nazwa1' OR Nazwa2='@_nazwa2';";
           */
            string sql =@"SELECT * from dbo.Users where Email = '"+Email+"';";
            var data= SqlDataAccess.LoadData<RegisterAccount>(sql);
            if (data.Count==0)
            {
                return true;
            }
            else
                return false;
        }


        public static string Logg (LoginAccount model)
        {

            HttpCookie Logged = new HttpCookie("Logged", "");
            HttpCookie RememberMe = new HttpCookie("RememberMe", "");
            switch (Loginto(model.Email, model.Password))
                {
                    case "Success":
                        {
                                if (model.RememberMe)
                                {
                                    Logged.Value = LoginDB(model);
                                    Logged.Expires.AddDays(10);
                                    HttpContext.Current.Response.SetCookie(Logged);
                                    RememberMe.Value = ("True");
                                    RememberMe.Expires.AddDays(10);
                                    HttpContext.Current.Response.SetCookie(RememberMe);
                                    HttpCookie Name = new HttpCookie("Name", "");
                                    Name.Value = Crypt(model.Email);
                                    Name.Expires.AddDays(10);
                                    HttpContext.Current.Response.SetCookie(Name);
                                    return "Success|True";
                                }
                                else
                                {
                                    Logged.Value = LoginDB(model);
                                    Logged.Expires.AddHours(1);
                                    HttpContext.Current.Response.SetCookie(Logged);
                                    RememberMe.Value = ("False");
                                    RememberMe.Expires.AddHours(1);
                                    HttpContext.Current.Response.SetCookie(RememberMe);
                                    HttpCookie Name = new HttpCookie("Name", "");
                                    Name.Value = Crypt(model.Email);
                                    Name.Expires.AddHours(1);
                                    HttpContext.Current.Response.SetCookie(Name);
                                    return "Success|False";
                                }
                    break;
                        }

                    case "Password":
                        {
                            Logged.Value = ("false");
                            Logged.Expires = DateTime.Now.AddDays(-1);
                            HttpContext.Current.Response.SetCookie(Logged);
                        return "Password";

                        break;
                        }

                    case "Email":
                        {
                            Logged.Value = ("false");
                            Logged.Expires = DateTime.Now.AddDays(-1);
                            HttpContext.Current.Response.SetCookie(Logged);
                        return "Email";
                    break;
                        }

                }


            return null;
        }



        public static string LoginDB(LoginAccount model)
        {
            model.Session = RandomIdSession(DateTime.Now.ToString());
            string sql = @"update dbo.Users set Session = '" + model.Session + "' WHERE Email= '" + model.Email + "' ;";
            SqlDataAccess.SaveData(sql, model);
            return model.Session;
        }
        public static string LoginDbFalse(LoginAccount model)
        {
            model.Session = "false";
            string sql = @"update dbo.Users set Session = '" + model.Session + "' WHERE Email= '" + model.Email + "' ;";
            SqlDataAccess.SaveData(sql, model);
            return model.Session;
        }
        

        #region Haszowanie
        public static string HashPasswordUsingPBKDF2(string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32, 10000);

            byte[] hash = rfc2898DeriveBytes.GetBytes(20);

            byte[] salt = rfc2898DeriveBytes.Salt;

            return salt.ToString() + "|" + hash.ToString();
        }


        public static string RandomIdSession(string seed)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(seed, 32, 10000);

            byte[] salt = rfc2898DeriveBytes.Salt;

            return Convert.ToBase64String(salt) ;
        }

        public static bool CorrectPassword(RegisterAccount account)
        {
            byte[] SaltByte = Convert.FromBase64String(account.Salt);
            var dupa = Convert.ToBase64String(SaltByte);
            var Hash = new Rfc2898DeriveBytes(account.Password, SaltByte,10000);
            //byte[] H2 = account.Hash;
            byte[] H1 = Convert.FromBase64String(account.Hash);
            var H2 = Hash.GetBytes(H1.Length);

            if (H1.SequenceEqual(H2))
            {
                return true;
            }
            else 
                return false;
        }
        #endregion

        
        #region Crypt
        public static string Crypt(this string text)    
        {
            
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(text));
        }

        public static string Decrypt(this string text)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(text));
        }
        #endregion


        
    }
}