using MusicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicApp.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            AdminViewModel adminData = new AdminViewModel();
            adminData.genreData = genreGroupBy();
            adminData.viewCountData = viewCountGroupBy();
            adminData.songs = db.Songs.ToList();
            adminData.artists = db.Artists.ToList();
            return View(adminData);
        }

        private String genreGroupBy() {
            String result = "[";
            double allSongs = db.Songs.ToList().Count();
            foreach (var genre in db.Songs.ToList().GroupBy(info => info.genre)
                        .Select(group => new {
                            label = group.Key,
                            Count = group.Count()
                        }))
            {
                result += "{\"label\":\""+genre.label+"\",\"count\":"+100*genre.Count/allSongs+"},";
            }
            if (result != "[")
            {
                result = result.Remove(result.Length - 1);
            }
            result += "]";
            return result;
        }
        private String viewCountGroupBy()
        {
            String result = "[";
            double allSongs = db.Songs.ToList().Sum(song=>song.numOfViews);
            if(allSongs == 0)
            {
                allSongs = 1;
            }

            foreach (var genre in db.Songs.ToList().GroupBy(info => info.genre)
                        .Select(group => new {
                            label = group.Key,
                            Count = group.Sum(gcount => gcount.numOfViews)
                        }))
            {
                result += "{\"label\":\"" + genre.label + "\",\"count\":" + 100 * genre.Count / allSongs + "},";
            }
            if (result != "[")
            {
                result = result.Remove(result.Length - 1);
            }
            result += "]";
            return result;
        }
    }
}