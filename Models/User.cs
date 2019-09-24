using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cexam.Models
{
    public class User 
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage="Name is required")]
        [MinLength(2, ErrorMessage="Name must be 2 characters or longer")]
        public string Name {get;set;}
        
        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage="Invalid Email")]
        public string Email {get;set;}
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
        ErrorMessage = "Invalid email format")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        [NotMapped]
        [Required(ErrorMessage="This is a required field.")]
        [Compare("Password", ErrorMessage="Password must match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}
        public List<List> Lists { get; set; }

        public List<Join> Attendees { get; set; }
    
    }
}