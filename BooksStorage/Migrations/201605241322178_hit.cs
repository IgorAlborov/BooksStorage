namespace BooksStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hits",
                c => new
                    {
                        HitId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                        BookId = c.Int(),
                    })
                .PrimaryKey(t => t.HitId)
                .ForeignKey("dbo.Books", t => t.BookId)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hits", "BookId", "dbo.Books");
            DropIndex("dbo.Hits", new[] { "BookId" });
            DropTable("dbo.Hits");
        }
    }
}
