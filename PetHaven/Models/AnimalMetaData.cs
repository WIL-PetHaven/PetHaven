using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{
    [MetadataType(typeof(AnimalMetaData))]
    public partial class Animal
    {
    }

    public class AnimalMetaData
    {
        [Display(Name = "Animal Name")]
        public string Name;
    }
}