namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relationshipbetweendepartmentanemployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "IdDepartment", c => c.Int());
            CreateIndex("dbo.Employees", "IdDepartment");
            AddForeignKey("dbo.Employees", "IdDepartment", "dbo.Departments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "IdDepartment", "dbo.Departments");
            DropIndex("dbo.Employees", new[] { "IdDepartment" });
            DropColumn("dbo.Employees", "IdDepartment");
        }
    }
}
