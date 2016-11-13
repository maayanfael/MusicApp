using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class Album : DbContext
    {
        [Key]
        [DisplayName("Album Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Album name")]
        public string AlbumName { get; set; }

        [Required]
        [DisplayName("Artist")]
        public int? artistId { get; set; }

        [DisplayName("Cover Photo")]
        public string coverPhoto { get; set; }

        [DisplayName("Songs")]
        public virtual List<Song> songs { get; set; }

        [DisplayName("Number Of views")]
        public int numOfViews { get; set; }

        virtual public Artist artist { get; set; }
    }
}
