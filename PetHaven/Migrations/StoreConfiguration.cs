namespace PetHaven.Migrations.StoreConfiguration
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using System.Collections.Generic;
    using System;
    internal sealed class StoreConfiguration : DbMigrationsConfiguration<PetHaven.DAL.StoreContext>
    {
        public StoreConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PetHaven.DAL.StoreContext context)
        {
            var categories = new List<Category>
            {
                
            };
            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

			var Animals = new List<Animal>
			{

			};

			Animals.ForEach(c => context.Animals.AddOrUpdate(p => p.Name, c));
			context.SaveChanges();

			var images = new List<AnimalImage>
            {
                
            };

            images.ForEach(c => context.AnimalImages.AddOrUpdate(p => p.FileName, c));
            context.SaveChanges();

            var imageMappings = new List<AnimalImageMapping>
            {
               
            };

            imageMappings.ForEach(c => context.AnimalImageMappings.AddOrUpdate(im => im.AnimalImageID, c));
            context.SaveChanges();

                    
            context.SaveChanges();


        }
    }
}
