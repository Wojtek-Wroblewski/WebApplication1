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


namespace WebApplication2.DBConnect
{
    public class Register
    {

        public static UserAccount NewAccount (UserAccount model)
        {
            string HashSalt = HashPasswordUsingPBKDF2(model.Password);
            string[] temp = HashSalt.Split('|');

            UserAccount data = new UserAccount 
            {
                FirstName =model.FirstName,
                LastName=model.LastName,
                Email=model.Email,
                UserName=model.UserName,
                hash = temp[1],
                salt = temp[0]
            };

            string sql1 = @"insert into dbo.Users (FirstName,LastName,Email,UserName,hash,salt) values (@FirstName,@LastName,@Email,@UserName,@hash,@salt);";
            SqlDataAccess.SaveData(sql1, data);

            if (EmailFree(data.Email))
            {
                string sql = @"insert into dbo.Users (FirstName,LastName,Email,UserName,hash,salt) values (@FirstName,@LastName,@Email,@UserName,@hash,@salt);";
                SqlDataAccess.SaveData(sql, data);
                return data;
            }
            else
            {
                return null;
            }
        }

        public static bool EmailFree(string Email)
        {/*@"select * from dbo.Artysci where ArtId='" + Id+"';";// where Nazwa1='@_nazwa1' OR Nazwa2='@_nazwa2';";
           */
            string sql =@"SELECT * from dbo.Users where Email = '"+Email+"';";
            var data= SqlDataAccess.LoadData<UserAccount>(sql);
            if (data!=null)
            {
                return false;
            }
            else
                return true;
        }




        #region Haszowanie
        public static string HashPasswordUsingPBKDF2(string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32)
            {
                IterationCount = 10000
            };

            byte[] hash = rfc2898DeriveBytes.GetBytes(20);

            byte[] salt = rfc2898DeriveBytes.Salt;

            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
        }
        #endregion
    }
}