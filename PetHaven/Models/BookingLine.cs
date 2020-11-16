using System;
using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{
    public class BookingLine
    {
        public int ID { get; set; }
        public String BookingID { get; set; }
        public int AnimalID { get; set; }
        [Range(0, 1, ErrorMessage = "Change to no quantity")]
        public int Quantity { get; set; }
        public String AnimalName { get; set; }
        public String AnimalDescrtiption { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Animal Animal { get; set; }

    }
}