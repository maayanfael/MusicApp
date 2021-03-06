﻿using System;
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
    public class ArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artists
        public ActionResult Index()
        {
            return View(db.Artists.ToList());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            artist.numOfViews = artist.numOfViews + 1;
            db.Entry(artist).State = EntityState.Modified;
            db.SaveChanges();
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artists/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {   
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,firstName,lastName, biography")] Artist artist, HttpPostedFileBase picture)
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
                    artist.picture = base64String;
                }

                artist.numOfViews = 0;

                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,firstName,lastName, biography")] Artist artist, HttpPostedFileBase picture)
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
                    artist.picture = base64String;
                }

                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
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
