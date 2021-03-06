﻿using System;
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

        [Display(Name = "Animal Name")]
        public string AnimalName { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Booking")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd 0:HH:mmm:sss}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBooking { get; set; }

        public List<BookingsLine> BookingsLines { get; set; }
    }
}