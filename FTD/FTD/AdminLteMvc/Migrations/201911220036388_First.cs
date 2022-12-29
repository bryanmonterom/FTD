namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IdRequest = c.Int(nullable: false),
                        ByteContent = c.Binary(),
                        ContentType = c.String(),
                        IdAttachmentType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttachmentTypes", t => t.IdAttachmentType, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .Index(t => t.IdRequest)
                .Index(t => t.IdAttachmentType);
            
            CreateTable(
                "dbo.AttachmentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FlowSteps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdEmployee = c.Int(nullable: false),
                        StepNumber = c.Int(nullable: false),
                        StepName = c.String(),
                        IdFlowType = c.Int(nullable: false),
                        IsAttachmentNeed = c.Boolean(nullable: false),
                        IsMatrixNeeded = c.Boolean(nullable: false),
                        IsTransferNeed = c.Boolean(nullable: false),
                        IsSupervisorNeed = c.Boolean(nullable: false),
                        IsBuyersBlocker = c.Boolean(nullable: false),
                        IdGroup = c.Int(true),
                        IdAttachmentType = c.Int(true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttachmentTypes", t => t.IdAttachmentType)
                .ForeignKey("dbo.Employees", t => t.IdEmployee, cascadeDelete: true)
                .ForeignKey("dbo.FlowTypes", t => t.IdFlowType, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.IdGroup)
                .Index(t => t.IdEmployee)
                .Index(t => t.IdFlowType)
                .Index(t => t.IdGroup)
                .Index(t => t.IdAttachmentType);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identification = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        IdSupervisor = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdSupervisor)
                .Index(t => t.IdSupervisor);
            
            CreateTable(
                "dbo.EmployeeGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdEmployee = c.Int(nullable: false),
                        IdGroup = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdEmployee, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.IdGroup, cascadeDelete: true)
                .Index(t => t.IdEmployee)
                .Index(t => t.IdGroup);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTo = c.Int(nullable: false),
                        IdFrom = c.Int(nullable: false),
                        Identifier = c.String(),
                        IdRequestType = c.Int(nullable: false),
                        IdRequestor = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Subject = c.String(),
                        Message = c.String(),
                        IdStatus = c.Int(nullable: false),
                        IdFlowType = c.Int(nullable: false),
                        IdFlowSteps = c.Int(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                        From_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlowSteps", t => t.IdFlowSteps)
                .ForeignKey("dbo.FlowTypes", t => t.IdFlowType, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.From_Id)
                .ForeignKey("dbo.Employees", t => t.IdRequestor, cascadeDelete: true)
                .ForeignKey("dbo.RequestTypes", t => t.IdRequestType, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.IdStatus, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.IdTo, cascadeDelete: true)
                .Index(t => t.IdTo)
                .Index(t => t.IdRequestType)
                .Index(t => t.IdRequestor)
                .Index(t => t.IdStatus)
                .Index(t => t.IdFlowType)
                .Index(t => t.IdFlowSteps)
                .Index(t => t.From_Id);
            
            CreateTable(
                "dbo.FlowTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRequest = c.Int(nullable: false),
                        DateApproval = c.DateTime(nullable: false),
                        IdEmployee = c.Int(nullable: false),
                        Comments = c.String(nullable: false),
                        IdStatus = c.Int(nullable: false),
                        IdFlowSteps = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdEmployee, cascadeDelete: true)
                .ForeignKey("dbo.FlowSteps", t => t.IdFlowSteps)
                .ForeignKey("dbo.Requests", t => t.IdRequest)
                .ForeignKey("dbo.Status", t => t.IdStatus, cascadeDelete: true)
                .Index(t => t.IdRequest)
                .Index(t => t.IdEmployee)
                .Index(t => t.IdStatus)
                .Index(t => t.IdFlowSteps);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuditTrails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PerformedBy = c.String(),
                        Date = c.DateTime(nullable: false),
                        Action = c.String(),
                        ActionMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "IdRequest", "dbo.Requests");
            DropForeignKey("dbo.Attachments", "IdAttachmentType", "dbo.AttachmentTypes");
            DropForeignKey("dbo.FlowSteps", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.FlowSteps", "IdFlowType", "dbo.FlowTypes");
            DropForeignKey("dbo.FlowSteps", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.Requests", "IdTo", "dbo.Departments");
            DropForeignKey("dbo.Requests", "IdStatus", "dbo.Status");
            DropForeignKey("dbo.Requests", "IdRequestType", "dbo.RequestTypes");
            DropForeignKey("dbo.Requests", "IdRequestor", "dbo.Employees");
            DropForeignKey("dbo.RequestHistories", "IdStatus", "dbo.Status");
            DropForeignKey("dbo.RequestHistories", "IdRequest", "dbo.Requests");
            DropForeignKey("dbo.RequestHistories", "IdFlowSteps", "dbo.FlowSteps");
            DropForeignKey("dbo.RequestHistories", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.Requests", "From_Id", "dbo.Departments");
            DropForeignKey("dbo.Requests", "IdFlowType", "dbo.FlowTypes");
            DropForeignKey("dbo.Requests", "IdFlowSteps", "dbo.FlowSteps");
            DropForeignKey("dbo.Employees", "IdSupervisor", "dbo.Employees");
            DropForeignKey("dbo.EmployeeGroups", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.EmployeeGroups", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.FlowSteps", "IdAttachmentType", "dbo.AttachmentTypes");
            DropIndex("dbo.RequestHistories", new[] { "IdFlowSteps" });
            DropIndex("dbo.RequestHistories", new[] { "IdStatus" });
            DropIndex("dbo.RequestHistories", new[] { "IdEmployee" });
            DropIndex("dbo.RequestHistories", new[] { "IdRequest" });
            DropIndex("dbo.Requests", new[] { "From_Id" });
            DropIndex("dbo.Requests", new[] { "IdFlowSteps" });
            DropIndex("dbo.Requests", new[] { "IdFlowType" });
            DropIndex("dbo.Requests", new[] { "IdStatus" });
            DropIndex("dbo.Requests", new[] { "IdRequestor" });
            DropIndex("dbo.Requests", new[] { "IdRequestType" });
            DropIndex("dbo.Requests", new[] { "IdTo" });
            DropIndex("dbo.EmployeeGroups", new[] { "IdGroup" });
            DropIndex("dbo.EmployeeGroups", new[] { "IdEmployee" });
            DropIndex("dbo.Employees", new[] { "IdSupervisor" });
            DropIndex("dbo.FlowSteps", new[] { "IdAttachmentType" });
            DropIndex("dbo.FlowSteps", new[] { "IdGroup" });
            DropIndex("dbo.FlowSteps", new[] { "IdFlowType" });
            DropIndex("dbo.FlowSteps", new[] { "IdEmployee" });
            DropIndex("dbo.Attachments", new[] { "IdAttachmentType" });
            DropIndex("dbo.Attachments", new[] { "IdRequest" });
            DropTable("dbo.AuditTrails");
            DropTable("dbo.RequestTypes");
            DropTable("dbo.Status");
            DropTable("dbo.RequestHistories");
            DropTable("dbo.Departments");
            DropTable("dbo.FlowTypes");
            DropTable("dbo.Requests");
            DropTable("dbo.Groups");
            DropTable("dbo.EmployeeGroups");
            DropTable("dbo.Employees");
            DropTable("dbo.FlowSteps");
            DropTable("dbo.AttachmentTypes");
            DropTable("dbo.Attachments");
        }
    }
}
