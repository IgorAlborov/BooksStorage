using BooksStorage.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BooksStorage.Controllers.API
{
    public class ChartsController : ApiController
    {
        private BooksStorageContext db = new BooksStorageContext();
        [HttpGet]
        public HttpResponseMessage Hits(int id)
        {
            System.Threading.Thread.Sleep(1000);
            var book = db.Books.ToList().Find(x => x.Id == id);
            if (book == null) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Book is missed");
            }
            var data = book.GoogleChartData;
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}
