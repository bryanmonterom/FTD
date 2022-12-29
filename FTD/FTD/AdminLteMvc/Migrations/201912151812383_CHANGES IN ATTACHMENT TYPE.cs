namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CHANGESINATTACHMENTTYPE : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attachments", "IdAttachmentType", "dbo.AttachmentTypes");
            DropIndex("dbo.Attachments", new[] { "IdAttachmentType" });
            AlterColumn("dbo.Attachments", "IdAttachmentType", c => c.Int());
            CreateIndex("dbo.Attachments", "IdAttachmentType");
            AddForeignKey("dbo.Attachments", "IdAttachmentType", "dbo.AttachmentTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "IdAttachmentType", "dbo.AttachmentTypes");
            DropIndex("dbo.Attachments", new[] { "IdAttachmentType" });
            AlterColumn("dbo.Attachments", "IdAttachmentType", c => c.Int(nullable: false));
            CreateIndex("dbo.Attachments", "IdAttachmentType");
            AddForeignKey("dbo.Attachments", "IdAttachmentType", "dbo.AttachmentTypes", "Id", cascadeDelete: true);
        }
    }
}
