namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class REQESYPEREOVD : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "IdRequestType", "dbo.RequestTypes");
            DropIndex("dbo.Requests", new[] { "IdRequestType" });
            RenameColumn(table: "dbo.Requests", name: "IdRequestType", newName: "RequestType_Id");
            AlterColumn("dbo.Requests", "RequestType_Id", c => c.Int());
            CreateIndex("dbo.Requests", "RequestType_Id");
            AddForeignKey("dbo.Requests", "RequestType_Id", "dbo.RequestTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "RequestType_Id", "dbo.RequestTypes");
            DropIndex("dbo.Requests", new[] { "RequestType_Id" });
            AlterColumn("dbo.Requests", "RequestType_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Requests", name: "RequestType_Id", newName: "IdRequestType");
            CreateIndex("dbo.Requests", "IdRequestType");
            AddForeignKey("dbo.Requests", "IdRequestType", "dbo.RequestTypes", "Id", cascadeDelete: true);
        }
    }
}
