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
            string HashSalt = HashPasswordUsingPBKDF2(model.Password);
            string[] temp = HashSalt.Split('|');
            Random rng = new Random();
            RegisterAccount data = new RegisterAccount
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                Hash = temp[1],
                Salt = temp[0],
                Session = RandomIdSession(HashSalt)
            };

            if (EmailFree(data.Email))
            {
                string sql = @"insert into dbo.Users (FirstName,LastName,Email,UserName,hash,salt,Session) values (@FirstName,@LastName,@Email,@UserName,@Hash,@Salt,@Session);";
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
            var data= SqlDataAccess.LoadData<RegisterAccount>(sql);
            if (data.Count==0)
            {
                return true;
            }
            else
                return false;
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