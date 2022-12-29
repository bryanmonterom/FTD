namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fieldaddedfordepatet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "Identifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "Identifier");
        }
    }
}
