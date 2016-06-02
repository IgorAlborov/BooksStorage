using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BooksStorage.Models
{
    public class Hit
    {
        [Key]
        [Column("HitId")]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public int? BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}