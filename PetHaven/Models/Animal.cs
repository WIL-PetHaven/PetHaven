using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{
    public partial class Animal
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
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<AnimalImageMapping> AnimalImageMappings { get; set; }
    }
}
