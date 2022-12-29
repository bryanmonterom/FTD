namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forcingwiththedepartments : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Requests", new[] { "From_Id" });
            DropColumn("dbo.Requests", "IdFrom");
            RenameColumn(table: "dbo.Requests", name: "From_Id", newName: "IdFrom");
            AlterColumn("dbo.Requests", "IdFrom", c => c.Int(nullable: false));
            CreateIndex("dbo.Requests", "IdFrom");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Requests", new[] { "IdFrom" });
            AlterColumn("dbo.Requests", "IdFrom", c => c.Int());
            RenameColumn(table: "dbo.Requests", name: "IdFrom", newName: "From_Id");
            AddColumn("dbo.Requests", "IdFrom", c => c.Int(nullable: false));
            CreateIndex("dbo.Requests", "From_Id");
        }
    }
}
