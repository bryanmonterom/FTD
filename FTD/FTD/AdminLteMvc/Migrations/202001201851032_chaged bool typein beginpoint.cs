namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chagedbooltypeinbeginpoint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FlowSteps", "IsBeginPointWhenSupervisor", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FlowSteps", "IsBeginPointWhenSupervisor", c => c.Boolean());
        }
    }
}
