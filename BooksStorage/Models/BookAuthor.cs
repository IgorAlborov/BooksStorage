using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStorage.Models
{
    public class BookAuthor
    {
        public BookAuthor() {
            Books = new HashSet<Book>();
        }

        [Key]
        [Column("AuthorId")]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Author must between 2 and 20", MinimumLength = 2)]
        public string Author { get; set; }

        public virtual ICollection<Book> Books { get; set; }
              
    }
}