using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLibrary.DataAccess;
using WebApplication2.Models;
using System.Security.Cryptography;
using System.Text;

namespace System.Web.Mvc
    {
        public static class HelperModel
        {
            public static bool Logged(this HtmlHelper html, string Session,string Email)
            {

            string sql = @"SELECT * from dbo.Users where Session = '" + Session + "';";
            string DecrypeEmail = Decrypt(Email);
            var data = SqlDataAccess.LoadData<RegisterAccount>(sql);
            if (data != null)
                {
                    if (data[0].Email == DecrypeEmail)
                    { 
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            else
                {
                    return false;
                }

            }
        public static string Name(this HtmlHelper html, string Session, string Email)
        {
            string sql = @"SELECT * from dbo.Users where Session = '" + Session + "';";
            string DecrypeEmail = Decrypt(Email);
            var data = SqlDataAccess.LoadData<RegisterAccount>(sql);
            if (data != null)
            {
                if (data[0].Email == DecrypeEmail)
                {
                    return data[0].UserName;
                }
                else
                {
                    return "data[0].Email == DecrypeEmail";
                }
            }
            else
            {
                return "data != null";
            }
        }
        
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