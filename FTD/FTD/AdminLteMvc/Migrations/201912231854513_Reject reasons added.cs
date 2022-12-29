namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rejectreasonsadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReasonToRejects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RequestHistories", "IdRejectReasons", c => c.Int());
            CreateIndex("dbo.RequestHistories", "IdRejectReasons");
            AddForeignKey("dbo.RequestHistories", "IdRejectReasons", "dbo.ReasonToRejects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestHistories", "IdRejectReasons", "dbo.ReasonToRejects");
            DropIndex("dbo.RequestHistories", new[] { "IdRejectReasons" });
            DropColumn("dbo.RequestHistories", "IdRejectReasons");
            DropTable("dbo.ReasonToRejects");
        }
    }
}
