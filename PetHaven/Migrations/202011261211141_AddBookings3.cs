namespace PetHaven.Migrations.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookings3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "DateOfBooking", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookings", "DateOfBooking", c => c.DateTime());
        }
    }
}
