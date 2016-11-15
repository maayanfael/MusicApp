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
    public class SongsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static string[] searchFilters = new string[3] { "", "", "" };

        // GET: Songs
        public ActionResult Index()
        {
            ViewBag.Artists = db.Artists.ToList();
            ViewBag.Albums = db.Albums.ToList();
            ViewBag.Genres = Enum.GetValues(typeof(Genre))
                            .Cast<Genre>()
                            .Select(v => v.ToString())
                            .ToList();
            ViewBag.Filters = searchFilters;

            if (!filtersExist())
            {
               return View(db.Songs.ToList());
            }
            else
            {
              IEnumerable<Song> displaySongs = db.Songs.ToList();

              if (searchFilters[0] != "")
              {
                  displaySongs = (from p in displaySongs
                                  where p.artistId.ToString() == searchFilters[0]
                                  select p).ToList();
              }
              if (searchFilters[1] != "")
              {
                  displaySongs = (from p in displaySongs
                                  where p.albumId.ToString() == searchFilters[1]
                                  select p).ToList();

              }
              if (searchFilters[2] != "")
              {
                  displaySongs = (from p in displaySongs
                                  where p.genre.ToString() == searchFilters[2]
                                  select p).ToList();
              }
               
                return View(displaySongs.ToList());
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

        // GET: Songs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            song.numOfViews = song.numOfViews + 1;
            db.Entry(song).State = EntityState.Modified;
            db.SaveChanges();
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        [Authorize(Roles = "Admin")]
        // GET: Songs/Create
        public ActionResult Create()
        {
            ViewBag.Artists = from p in db.Artists.ToList()
                              select new
                              {
                                  Id = p.Id,
                                  FullName = p.firstName + " " + p.lastName
                              };

            ViewBag.Albums = db.Albums.ToList();

            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public ActionResult Create([Bind(Include = "Id,songName,albumId,artistId,publishDate,genre")] Song song, HttpPostedFileBase picture, HttpPostedFileBase video)
        {
            if (ModelState.IsValid)
            {
                if (picture != null && picture.ContentLength > 0)
                {
                    byte[] pictureData;
                    using (var reader = new System.IO.BinaryReader(picture.InputStream))
                    {
                        pictureData = reader.ReadBytes(picture.ContentLength);
                    }
                    string base64String = Convert.ToBase64String(pictureData);
                    song.picture = base64String;
                }

                if (video != null && video.ContentLength > 0)
                {
                    byte[] videoData;
                    using (var reader = new System.IO.BinaryReader(video.InputStream))
                    {
                        videoData = reader.ReadBytes(video.ContentLength);
                    }
                    string base64String = Convert.ToBase64String(videoData);
                    song.video = base64String;
                }

                song.numOfViews = 0;

                db.Songs.Add(song);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(song);
        }
        ///
        // GET: Songs/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,songName,albumId,artistId,publishDate,genre")] Song song, HttpPostedFileBase picture, HttpPostedFileBase video)
        {
            if (ModelState.IsValid)
            {
                if (picture != null && picture.ContentLength > 0)
                {
                    byte[] pictureData;
                    using (var reader = new System.IO.BinaryReader(picture.InputStream))
                    {
                        pictureData = reader.ReadBytes(picture.ContentLength);
                    }
                    string base64String = Convert.ToBase64String(pictureData);
                    song.picture = base64String;
                }

                if (video != null && video.ContentLength > 0)
                {
                    byte[] videoData;
                    using (var reader = new System.IO.BinaryReader(video.InputStream))
                    {
                        videoData = reader.ReadBytes(video.ContentLength);
                    }
                    string base64String = Convert.ToBase64String(videoData);
                    song.video = base64String;
                }
                db.Entry(song).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(song);
        }

        // GET: Songs/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Song song = db.Songs.Find(id);
            db.Songs.Remove(song);
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
