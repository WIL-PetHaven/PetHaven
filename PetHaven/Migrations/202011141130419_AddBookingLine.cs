namespace PetHaven.Migrations.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingLine : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.AnimalImageMappings",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            ImageNumber = c.Int(nullable: false),
            //            AnimalID = c.Int(nullable: false),
            //            AnimalImageID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.Animals", t => t.AnimalID, cascadeDelete: true)
            //    .ForeignKey("dbo.AnimalImages", t => t.AnimalImageID, cascadeDelete: true)
            //    .Index(t => t.AnimalID)
            //    .Index(t => t.AnimalImageID);
            
            //CreateTable(
            //    "dbo.Animals",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 500),
            //            Description = c.String(nullable: false, maxLength: 500),
            //            CategoryID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.Categories", t => t.CategoryID)
            //    .Index(t => t.CategoryID);
            
            //CreateTable(
            //    "dbo.Categories",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 50),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.AnimalImages",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            FileName = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .Index(t => t.FileName, unique: true);
            
            CreateTable(
                "dbo.BookingLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookingID = c.String(),
                        AnimalID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        AnimalName = c.String(),
                        AnimalDescrtiption = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Animals", t => t.AnimalID, cascadeDelete: true)
                .Index(t => t.AnimalID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingLines", "AnimalID", "dbo.Animals");
            DropForeignKey("dbo.AnimalImageMappings", "AnimalImageID", "dbo.AnimalImages");
            DropForeignKey("dbo.Animals", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.AnimalImageMappings", "AnimalID", "dbo.Animals");
            DropIndex("dbo.BookingLines", new[] { "AnimalID" });
            DropIndex("dbo.AnimalImages", new[] { "FileName" });
            DropIndex("dbo.Animals", new[] { "CategoryID" });
            DropIndex("dbo.AnimalImageMappings", new[] { "AnimalImageID" });
            DropIndex("dbo.AnimalImageMappings", new[] { "AnimalID" });
            DropTable("dbo.BookingLines");
            DropTable("dbo.AnimalImages");
            DropTable("dbo.Categories");
            DropTable("dbo.Animals");
            DropTable("dbo.AnimalImageMappings");
        }
    }
}
