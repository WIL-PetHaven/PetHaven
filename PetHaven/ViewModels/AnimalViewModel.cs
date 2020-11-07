using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PetHaven.ViewModels
{
    public class AnimalViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "The Animal name cannot be blank")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Please enter a Animal name between 3 and 50 characters in length")]
        [RegularExpression(@"^[,:a-zA-Z 0-9\.\,\+\-./]*$", ErrorMessage = "Please enter a Animal name made up of letters and numbers only")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Animal description cannot be blank")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Please enter a Animal description between 10 and 200 characters in length")]
        [RegularExpression(@"^[,:a-zA-Z 0-9\.\,\+\-./]*$", ErrorMessage = "Please enter a Animal description made up of letters and numbers only")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The price cannot be blank")]
        [Range(0.10, 100000, ErrorMessage = "Please enter a price between 0.10 and 100000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "The price must be a number up to two decimal places")]

        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        public SelectList CategoryList { get; set; }
        public List<SelectList> ImageLists { get; set; }
        public string[] AnimalImages { get; set; }
    }
}
