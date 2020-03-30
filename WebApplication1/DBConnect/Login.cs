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
using System.Web.Mvc;

namespace WebApplication2.DBConnect
{
    public class Logging
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
                                Logged.Value = ("True");
                                Logged.Expires.AddDays(10);
                                HttpContext.Current.Response.SetCookie(Logged);
                                if (model.RememberMe)
                                {
                                    int ddd = RememberMetoDB(model);

                                    return "Success|True";
                                }
                                else
                                {
                                    RememberMe.Value = ("False");
                                    RememberMe.Expires = DateTime.Now.AddDays(-1);
                                    HttpContext.Current.Response.SetCookie(RememberMe);
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



        public static int RememberMetoDB(LoginAccount model)
        {
            model.Session = RandomIdSession(DateTime.Now.ToString());

            HttpCookie RememberMe = new HttpCookie("RememberMe", "");
            RememberMe.Value = (model.Session.ToString());
            RememberMe.Expires.AddDays(10);
            HttpContext.Current.Response.SetCookie(RememberMe);           

            string sql = @"update dbo.Users set Session = '"+model.Session+"' WHERE Email= '"+model.Email+"' ;";
            return SqlDataAccess.SaveData(sql, model);
        }

        public static int LoggedSucces(LoginAccount model)
        {
            string sql = @"update dbo.Users set ";
            return SqlDataAccess.SaveData(sql, model);
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
    }
}