namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class issupervisorchangedtonotnull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "IsSupervisor", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "IsSupervisor", c => c.Boolean());
        }
    }
}
