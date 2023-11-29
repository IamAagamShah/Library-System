namespace bookData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialMigration : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Books");
            AddColumn("dbo.Books", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Books", "ISBN", c => c.String());
            AddPrimaryKey("dbo.Books", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Books");
            AlterColumn("dbo.Books", "ISBN", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Books", "Id");
            AddPrimaryKey("dbo.Books", "ISBN");
        }
    }
}
