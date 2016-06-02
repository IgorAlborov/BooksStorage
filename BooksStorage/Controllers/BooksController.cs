using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BooksStorage.DataAccess;
using BooksStorage.Models;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;

namespace BooksStorage.Controllers
{
    public class BooksController : Controller
    {
        private BooksStorageContext db = new BooksStorageContext();

        // GET: Books
        [OutputCache(Duration = 120,
            Location = System.Web.UI.OutputCacheLocation.Server)]
        public async Task<ActionResult> Index()
        {
            var books = db.Books.Include(a => a.BookAuthor);
            return View(await books.ToListAsync());
        }

        public ActionResult IndexRemoveCache()
        {
            string path = Url.Action("Index", "Persons");
            Response.RemoveOutputCacheItem(path);
            return RedirectToAction("Index");
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) {
                return HttpNotFound();
            }
            Book book = await db.Books.FindAsync(id);
            if (book != null) {
                Hit hit = db.Hits.ToList().Find(x => x.BookId == book.Id && x.Date == DateTime.UtcNow.Date);
                if (hit == null) {
                    hit = new Hit() { Date = DateTime.UtcNow.Date, Count = 1, BookId = book.Id };
                    db.Hits.Add(hit);
                    await db.SaveChangesAsync();
                } else {
                    hit.Count += 1;
                    db.Entry(hit).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //return RedirectToAction("Index");
                }
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            SelectList authors = new SelectList(db.Authors, "Id", "Author");
            ViewBag.Authors = authors;
            return View();
        }

        // POST: Books/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ISBN,BookTitle,AuthorId")] Book book)
        {
            if (ModelState.IsValid) {
                book.BookAuthor = db.Authors.Find(book.AuthorId);
                db.Books.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null) {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ISBN,BookTitle")] Book book)
        {
            if (ModelState.IsValid) {
                db.Entry(book).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null) {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Book book = await db.Books.FindAsync(id);
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Dchart(int id)
        {

            var hits = db.Hits.ToList().FindAll(x => x.BookId == id &&
                     x.Date >= DateTime.UtcNow.Date.AddDays(-7) &&
                     x.Date <= DateTime.UtcNow.Date);

            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 700;
            chart.Height = 300;

            var area = new ChartArea();
            //настройка области диаграммы размеры и т.д.
            chart.ChartAreas.Add(area);

            //Создание и определение серии данных
            var series = new Series();
            foreach (var item in hits) {
                //series.Points.AddXY(item.Key, item.Value);
                series.Points.AddXY(item.Date, item.Count);
            }
            //series.Label = "#PERCENT{P0}";
            series.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            series.ChartType = SeriesChartType.Line;
            series["PieLabelStyle"] = "Outside";

            chart.Series.Add(series);

            var returnStream = new MemoryStream();
            chart.ImageType = ChartImageType.Png;
            chart.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
