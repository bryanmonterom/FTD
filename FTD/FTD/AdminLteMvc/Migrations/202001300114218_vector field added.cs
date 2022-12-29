namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vectorfieldadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "Vector", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "Vector");
        }
    }
}
