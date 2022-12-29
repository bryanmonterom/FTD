namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFieldforemploee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "IsSupervisor", c => c.Boolean());
            AddColumn("dbo.Employees", "FileName", c => c.String());
            AddColumn("dbo.Employees", "ByteContent", c => c.Binary());
            AddColumn("dbo.Employees", "ContentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "ContentType");
            DropColumn("dbo.Employees", "ByteContent");
            DropColumn("dbo.Employees", "FileName");
            DropColumn("dbo.Employees", "IsSupervisor");
        }
    }
}
