using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{
    public class BookingsLine
    {
        public int ID { get; set; }

        public int BookingsID { get; set; }

        public int? AnimalID { get; set; }

        public int Quantity { get; set; }

        public string AnimalName { get; set; }

        [Display(Name = "Animal Description")]
        public string AnimalDescription { get; set; }

        public virtual Animal Animal { get; set; }

        public virtual Bookings Bookings { get; set; }

    }
}