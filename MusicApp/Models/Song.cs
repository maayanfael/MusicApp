using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppProj2.Models
{
    public class Song
    {
        [Key]
        [DisplayName("Song Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Song Name")]
        public string songName { get; set; }

        [Required]
        [DisplayName("Artist")]
        public Artist artist { get; set; }

        [DisplayName("Album")]
        public string album { get; set; }

        [DisplayName("Publish Date")]
        public DateTime publishDate { get; set; }

        [DisplayName("Picture")]
        public string picture { get; set; }

        [DisplayName("Video")]
        public string video { get; set; }

        [DisplayName("Number Of views")]
        public int numOfViews { get; set; }

        [DisplayName("Genre")]
        public string genre { get; set; }
        
    }
}
