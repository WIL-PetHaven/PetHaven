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

			var products = new List<Product>
			{

			};

			products.ForEach(c => context.Products.AddOrUpdate(p => p.Name, c));
			context.SaveChanges();

			var images = new List<ProductImage>
            {
                
            };

            images.ForEach(c => context.ProductImages.AddOrUpdate(p => p.FileName, c));
            context.SaveChanges();

            var imageMappings = new List<ProductImageMapping>
            {
               
            };

            imageMappings.ForEach(c => context.ProductImageMappings.AddOrUpdate(im => im.ProductImageID, c));
            context.SaveChanges();

                    
            context.SaveChanges();


        }
    }
}
