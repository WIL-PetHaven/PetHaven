using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PetHaven.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Please make sure to enter in your full name.")]
        [MaxLength(50)]
        //[RegularExpression(@"^[,;a-zA-Z0-9'-'\s:-.#&!@$?%*()/|""+-~{}”’<>]$", ErrorMessage = "Please enter a name made up of letters only")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please make sure to enter in your e-mail address.")]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please make sure to enter in your phone number.")]
        [MaxLength(15)]
        [Phone]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please make sure to enter in your message.")]
        [MaxLength(250)]
        //[RegularExpression(@"^[,;a-zA-Z0-9'-'\s:-.#&!@$?%*()/|""+-~{}”’<>]$", ErrorMessage = "Please enter a message made up of letters and numbers only")]
        public string Message { get; set; }
    }
}