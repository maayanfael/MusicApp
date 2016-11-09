using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppProj2.Models
{
    public class Album
    {
        [Key]
        [DisplayName("Album Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Album name")]
        public string AlbumName { get; set; }

        [DisplayName("Cover Photo")]
        public string coverPhoto { get; set; }

        [DisplayName("Songs")]
        public virtual List<Song> songs { get; set; }

        [DisplayName("Number Of views")]
        public int numOfViews { get; set; }
    }
}
