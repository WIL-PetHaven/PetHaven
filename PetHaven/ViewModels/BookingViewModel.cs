using PetHaven.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetHaven.ViewModels
{
    public class BookingViewModel
    {
        public List<BookingLine> BookingLines { get; set; }

    }
}