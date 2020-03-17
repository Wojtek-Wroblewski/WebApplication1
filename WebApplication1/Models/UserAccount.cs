using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class UserAccount
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage ="Imię jest wymagane")]
        [Display(Name ="Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [Display(Name = "Nazwisko")]
        public string LastName{ get; set; }

        [Required(ErrorMessage = "E-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny adres e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "Podane hasła różnią się")]
        [Display(Name = "Powtórz e-mail")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagan")]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Podane hasła różnią się")]
        [Display(Name = "Powtórz hasło")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string hash { get; set; }
        public string salt { get; set; }


        public List <UserAccount> UserList= new List<UserAccount>();
            public List<UserAccount> ReturnList()
        {
            UserList.Add(new UserAccount()
            { UserID = 1, FirstName = "wojtek1FN", LastName = "dupa1", Email = "qwe@qwe.qwe1", UserName = "wojtek1", Password = "q1", ConfirmPassword = "g1" });
            
            return UserList;
        }
    }
}