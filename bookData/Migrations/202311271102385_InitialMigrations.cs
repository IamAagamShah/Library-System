namespace bookData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ISBN = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Author = c.String(),
                        Subtitle = c.String(),
                        Image = c.String(),
                        Description = c.String(),
                        Link = c.String(),
                        PublishDate = c.String(),
                        PageCount = c.Int(nullable: false),
                        Rating = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ISBN);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Books");
        }
    }
}
