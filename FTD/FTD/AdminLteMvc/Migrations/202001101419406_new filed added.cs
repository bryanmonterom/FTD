namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfiledadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlowSteps", "IdGroupToInform", c => c.Int());
            AddColumn("dbo.FlowSteps", "IsRestorePoint", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlowSteps", "SendEmailWhenCompleted", c => c.Boolean(nullable: false));
            CreateIndex("dbo.FlowSteps", "IdGroupToInform");
            AddForeignKey("dbo.FlowSteps", "IdGroupToInform", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlowSteps", "IdGroupToInform", "dbo.Groups");
            DropIndex("dbo.FlowSteps", new[] { "IdGroupToInform" });
            DropColumn("dbo.FlowSteps", "SendEmailWhenCompleted");
            DropColumn("dbo.FlowSteps", "IsRestorePoint");
            DropColumn("dbo.FlowSteps", "IdGroupToInform");
        }
    }
}
