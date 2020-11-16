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
        public DbSet<BookingLine> BookingLines { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<BookingsLine> BookingsLines { get; set; }
    }
}