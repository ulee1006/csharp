  
using System;
using System.ComponentModel.DataAnnotations;


namespace cexam.Models
{
    public class LoginUser 
    {
        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage="Invalid Email")]
        public string LoginEmail {get;set;}
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Invalid password.")]
        public string LoginPassword {get;set;}
    }
}