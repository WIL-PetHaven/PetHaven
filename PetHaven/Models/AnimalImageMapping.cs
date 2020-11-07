namespace PetHaven.Models
{
    public class AnimalImageMapping
    {
        public int ID { get; set; }
        public int ImageNumber { get; set; }
        public int AnimalID { get; set; }
        public int AnimalImageID { get; set; }

        public virtual Animal Animal { get; set; }
        public virtual AnimalImage AnimalImage { get; set; }
    }
}