using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class AnimalImage
    {
        public int ID { get; set; }
        [Display(Name = "File")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }

        public virtual ICollection<AnimalImageMapping> AnimalImageMappings { get; set; }
    }
}
