﻿using DataLibrary.DataAccess;
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

        public static RegisterAccount NewAccount (RegisterAccount model)
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
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32)
            {
                IterationCount = 10000
            };

            byte[] hash = rfc2898DeriveBytes.GetBytes(20);

            byte[] salt = rfc2898DeriveBytes.Salt;

            var d1 = Convert.ToBase64String(salt);
            byte[] d2 = Convert.FromBase64String(d1);
            var d3 = Convert.ToBase64String(d2);


            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
        }


        public static string RandomIdSession(string seed)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(seed, 32)
            {
                IterationCount = 10000
            };

            byte[] hash = rfc2898DeriveBytes.GetBytes(20);

            byte[] salt = rfc2898DeriveBytes.Salt;

            return Convert.ToBase64String(salt) ;
        }
        #endregion
    }
}