namespace PetHaven.Migrations.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookings1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "DateOfBooking", c => c.DateTime(nullable: false));
            DropColumn("dbo.Bookings", "DeliveryAddress_AddressLine1");
            DropColumn("dbo.Bookings", "DeliveryAddress_AddressLine2");
            DropColumn("dbo.Bookings", "DeliveryAddress_Town");
            DropColumn("dbo.Bookings", "DeliveryAddress_County");
            DropColumn("dbo.Bookings", "DeliveryAddress_Postcode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "DeliveryAddress_Postcode", c => c.String(nullable: false));
            AddColumn("dbo.Bookings", "DeliveryAddress_County", c => c.String(nullable: false));
            AddColumn("dbo.Bookings", "DeliveryAddress_Town", c => c.String(nullable: false));
            AddColumn("dbo.Bookings", "DeliveryAddress_AddressLine2", c => c.String());
            AddColumn("dbo.Bookings", "DeliveryAddress_AddressLine1", c => c.String(nullable: false));
            DropColumn("dbo.Bookings", "DateOfBooking");
        }
    }
}
