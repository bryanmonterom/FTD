namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class directoradded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "IdDirector", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "IdDirector");
        }
    }
}
