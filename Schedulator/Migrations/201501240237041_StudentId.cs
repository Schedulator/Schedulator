namespace Schedulator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StudentId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "StudentId");
        }
    }
}
