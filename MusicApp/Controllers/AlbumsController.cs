using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicApp.Models;

namespace MusicApp.Controllers
{
    public class AlbumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static string[] searchFilters = new string[3] { "", "", "" };

        // GET: Albums
        public ActionResult Index()
        {
            ViewBag.Artists = db.Artists.ToList();
            ViewBag.NumOfViews = new string[] { "0-10", "11-100", "101-1000", "1000+" };
            ViewBag.NumOfSongsInAlbum = new string[] { "1-5", "6-10", "11-15", "15+" };
            ViewBag.Filters = searchFilters;

            if (!filtersExist())
            {
                return View(db.Albums.ToList());
            }
            else
            {
                IEnumerable<Album> displayAlbums = db.Albums.ToList();

                if (searchFilters[0] != "")
                {
                    displayAlbums = (from p in displayAlbums
                                     where p.artistId.ToString() == searchFilters[0]
                                    select p).ToList();
                }
                if (searchFilters[1] != "")
                {
                    string[] split;
                    if (searchFilters[1].Contains("-"))
                    {
                        split = searchFilters[1].Split('-');
                        displayAlbums = (from p in displayAlbums
                                         where p.numOfViews >= Int32.Parse(split[0]) && p.numOfViews <= Int32.Parse(split[1])
                                         select p).ToList();
                    }

                    if (searchFilters[1].Contains("+"))
                    {
                        split = searchFilters[1].Split('+');
                        displayAlbums = (from p in displayAlbums
                                         where p.numOfViews > Int32.Parse(split[0])
                                         select p).ToList();
                    }

                }
                if (searchFilters[2] != "")
                {
                    string[] split;
                    if (searchFilters[2].Contains("-"))
                    {
                        split = searchFilters[2].Split('-');
                        displayAlbums = (from p in displayAlbums
                                         where p.songs.Count >= Int32.Parse(split[0]) && p.numOfViews <= Int32.Parse(split[1])
                                         select p).ToList();
                    }

                    if (searchFilters[2].Contains("+"))
                    {
                        split = searchFilters[2].Split('+');
                        displayAlbums = (from p in displayAlbums
                                         where p.songs.Count > Int32.Parse(split[0])
                                         select p).ToList();
                    }
                }

                return View(displayAlbums.ToList());
            }
        }

        public ActionResult setFilter(string value, int pos)
        {
            searchFilters[pos] = value;
            return RedirectToAction("Index");
        }

        public ActionResult resetFilters()
        {
            searchFilters = new string[3] { "", "", "" };
            return RedirectToAction("Index");
        }

        private Boolean filtersExist()
        {
            foreach (string filter in searchFilters)
            {
                if (filter != "")
                {
                    return true;
                }
            }

            return false;
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            album.numOfViews = album.numOfViews + 1;
            db.Entry(album).State = EntityState.Modified;
            db.SaveChanges();
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Artists = from p in db.Artists.ToList()
                              select new
                              {
                                  Id = p.Id,
                                  FullName = p.firstName + " " + p.lastName
                              };

            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,artistId, AlbumName")] Album album, HttpPostedFileBase coverPhoto)
        {
            if (ModelState.IsValid)
            {
                if (coverPhoto != null && coverPhoto.ContentLength > 0)
                {
                    byte[] pictureData;
                    using (var reader = new System.IO.BinaryReader(coverPhoto.InputStream))
                    {
                        pictureData = reader.ReadBytes(coverPhoto.ContentLength);
                    }
                    string base64String = Convert.ToBase64String(pictureData);
                    album.coverPhoto = base64String;
                }

                album.numOfViews = 0;

                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(album);
        }

        // GET: Albums/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);

            ViewBag.Artists = from p in db.Artists.ToList()
                              select new
                              {
                                  Id = p.Id,
                                  FullName = p.firstName + " " + p.lastName
                              };


            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,AlbumName")] Album album, HttpPostedFileBase coverPhoto)
        {
            if (ModelState.IsValid)
            {
                if (coverPhoto != null && coverPhoto.ContentLength > 0)
                {
                    byte[] pictureData;
                    using (var reader = new System.IO.BinaryReader(coverPhoto.InputStream))
                    {
                        pictureData = reader.ReadBytes(coverPhoto.ContentLength);
                    }
                    string base64String = Convert.ToBase64String(pictureData);
                    album.coverPhoto = base64String;
                }

                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(album);
        }

        // GET: Albums/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
