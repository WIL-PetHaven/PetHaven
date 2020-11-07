namespace PetHaven.Migrations.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class twocontextreset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Animals",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 500),
                    Description = c.String(nullable: false, maxLength: 500),
                    CategoryID = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .Index(t => t.CategoryID);

            CreateTable(
                "dbo.AnimalImageMappings",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ImageNumber = c.Int(nullable: false),
                    AnimalID = c.Int(nullable: false),
                    AnimalImageID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Animals", t => t.AnimalID, cascadeDelete: true)
                .ForeignKey("dbo.AnimalImages", t => t.AnimalImageID, cascadeDelete: true)
                .Index(t => t.AnimalID)
                .Index(t => t.AnimalImageID);

            CreateTable(
                "dbo.AnimalImages",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    FileName = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.FileName, unique: true);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AnimalImageMappings", "AnimalImageID", "dbo.AnimalImages");
            DropForeignKey("dbo.AnimalImageMappings", "AnimalID", "dbo.Animals");
            DropForeignKey("dbo.Animals", "CategoryID", "dbo.Categories");
            DropIndex("dbo.AnimalImages", new[] { "FileName" });
            DropIndex("dbo.AnimalImageMappings", new[] { "AnimalImageID" });
            DropIndex("dbo.AnimalImageMappings", new[] { "AnimalID" });
            DropIndex("dbo.Animals", new[] { "CategoryID" });
            DropTable("dbo.AnimalImages");
            DropTable("dbo.AnimalImageMappings");
            DropTable("dbo.Animals");
            DropTable("dbo.Categories");
        }
    }
}