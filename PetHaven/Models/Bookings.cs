using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{    
    public class Bookings
    {
        [Display(Name = "Bookings ID")]
        public int BookingsID { get; set; }

        [Display(Name = "User")]
        public string UserID { get; set; }

        [Display(Name = "Booking By")]
        public string DeliveryName { get; set; }

        [Display(Name = "User Address")]
        public Address DeliveryAddress { get; set; }

        [Display(Name = "Animal Jake")]
        public string AnimalName { get; set; }

        [Display(Name = "Booking Date")]
        public DateTime DateCreated { get; set; }

        public List<BookingsLine> BookingsLines { get; set; }
    }
}