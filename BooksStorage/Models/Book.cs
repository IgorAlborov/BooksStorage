using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Google.DataTable.Net.Wrapper;

namespace BooksStorage.Models
{
   public class Book
    {
        public Book()
        {
            Hits = new HashSet<Hit>();
        }

        [Key]
        [Column("BookId")]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(97(7|8)-\d-\d{2,7}-\d{1,6}-\w)$", ErrorMessage = "ISBN must be 97[7|8]-1-11-1-x")]
        public string ISBN { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "BookTitle must between 1 and 100", MinimumLength = 1)]
        public string BookTitle { get; set; }

        public int? AuthorId { get; set; }
        public virtual BookAuthor BookAuthor { get; set; }

        public virtual ICollection<Hit> Hits { get; set; }

        public string GoogleChartData {
            get {

                //let's instantiate the DataTable.
                var dt = new Google.DataTable.Net.Wrapper.DataTable();
                dt.AddColumn(new Column(ColumnType.Date, "Day", "Day"));
                dt.AddColumn(new Column(ColumnType.Number, "Count", "Count"));

                foreach (Hit hit in Hits) {
                    Row r = dt.NewRow();
                    r.AddCellRange(new Cell[]
                    {
                        new Cell(hit.Date),
                        new Cell(hit.Count)
                    });
                    dt.AddRow(r);
                }

                //Let's create a Json string as expected by the Google Charts API.
                return dt.GetJson();
            }
        }
    }
}