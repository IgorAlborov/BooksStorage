namespace BooksStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookAuthors",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        Author = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        ISBN = c.String(nullable: false),
                        BookTitle = c.String(nullable: false, maxLength: 100),
                        AuthorId = c.Int(),
                        BookAuthor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.BookAuthors", t => t.BookAuthor_Id)
                .Index(t => t.BookAuthor_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "BookAuthor_Id", "dbo.BookAuthors");
            DropIndex("dbo.Books", new[] { "BookAuthor_Id" });
            DropTable("dbo.Books");
            DropTable("dbo.BookAuthors");
        }
    }
}
