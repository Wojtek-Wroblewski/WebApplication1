using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class RegisterAccount
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [EmailAddress(ErrorMessage = "Enter correct e-mail adress")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "E-mail are different")]
        [Display(Name = "Confirm e-mail")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password are different")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Session { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        
    }
    public class LoginAccount
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [EmailAddress(ErrorMessage = "Enter correct e-mail adress")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Session { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }


    }
}