using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BooksStorage.Models;

namespace BooksStorage.DataAccess
{
    public class BookDbInitializer : CreateDatabaseIfNotExists<BooksStorageContext>
    {
        protected override void Seed(BooksStorageContext db)
        {
            var author = new BookAuthor { Author = "Pushkin" };
            db.Authors.Add(author);
            db.Books.Add(new Book { ISBN = "977-1-11-1-a", BookTitle = "Evgeniy Onegin",BookAuthor = author});
            author = new BookAuthor { Author = "Lermontov" };
            db.Authors.Add(author);
            db.Books.Add(new Book { ISBN = "978-2-51-1-b", BookTitle = "Borodino", BookAuthor = author });
            author = new BookAuthor { Author = "Tolstoy" };
            db.Authors.Add(author);
            db.Books.Add(new Book { ISBN = "977-3-41-1-c", BookTitle = "War&Peace", BookAuthor = author });
            author = new BookAuthor { Author = "Dostoevskiy" };
            db.Authors.Add(author);
            db.Books.Add(new Book { ISBN = "978-4-31-1-d", BookTitle = "Idiot", BookAuthor = author });
            author = new BookAuthor { Author = "Gogol" };
            db.Authors.Add(author);
            db.Books.Add(new Book { ISBN = "977-5-21-1-f", BookTitle = "Viy", BookAuthor = author });
            Random r = new Random();
            for (var i = 1; i <= 5; i++) {
                for (var j = 6; j > 0; j--) {
                    db.Hits.Add(new Hit { Date = DateTime.UtcNow.Date.AddDays(-j), Count = r.Next(1, 100),BookId=i });
                }
            }

            base.Seed(db);
        }

       
    }
}