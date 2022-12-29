namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NEWFIELDFORBGINPOINTINFLOWSTEPS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlowSteps", "IsBeginPointWhenSupervisor", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FlowSteps", "IsBeginPointWhenSupervisor");
        }
    }
}
