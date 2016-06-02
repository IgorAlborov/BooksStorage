using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BooksStorage.Models;

namespace BooksStorage.DataAccess
{
    public class BooksStorageContext : DbContext
    {
        public BooksStorageContext() : base("BooksStorage")
        {
            Database.SetInitializer(new BookDbInitializer());
            //    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BooksStorageContext>());
        }



        public virtual DbSet<BookAuthor> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Hit> Hits { get; set; }

    }
}