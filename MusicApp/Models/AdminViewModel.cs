using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class AdminViewModel
    {
        public String genreData { get; set; }
        public String viewCountData { get; set; }
        public List<Song> songs { get; set; }
        public List<Artist> artists { get; set; }

    }
}