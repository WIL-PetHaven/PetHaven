using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{
    public class ContactForm
    {
        [Required]
        [RegularExpression(@"^[,;a-zA-Z0-9'-'\s:-.#&!@$?%*()/|""+-~{}”’<>]$", ErrorMessage = "Please enter a name made up of letters only")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [RegularExpression(@"^[,;a-zA-Z0-9'-'\s:-.#&!@$?%*()/|""+-~{}”’<>]$", ErrorMessage = "Please enter a message made up of letters and numbers only")]
        public string Message { get; set; }
    }
}