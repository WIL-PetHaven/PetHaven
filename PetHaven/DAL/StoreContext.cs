using PetHaven.Models;
using System.Data.Entity;

namespace PetHaven.DAL
{
    public class StoreContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AnimalImage> AnimalImages { get; set; }
        public DbSet<AnimalImageMapping> AnimalImageMappings { get; set; }

        public System.Data.Entity.DbSet<PetHaven.Models.Booking> Bookings { get; set; }
    }
}