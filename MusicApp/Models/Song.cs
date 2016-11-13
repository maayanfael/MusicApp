using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public enum Genre
    {
        AvantGarde,
        Blues,
        CaribbeanAndCaribbeanInfluenced,
        Comedy,
        Country,
        EasyListening,
        Electronic,
        Folk,
        HipHop,
        Jazz,
        Latin,
        Pop,
        RandBandSoul,
        Rock
    }

    public class Song : DbContext
    {
        [Key]
        [DisplayName("Song Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Song Name")]
        public string songName { get; set; }
        
        [Required]
        [DisplayName("Artist")]
        public int artistId { get; set; }

        [DisplayName("Album")]
        public int albumId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Publish Date")]
        public DateTime publishDate { get; set; }

        [DisplayName("Picture")]
        public string picture { get; set; }

        [DisplayName("Video")]
        public string video { get; set; }

        [DisplayName("Number Of views")]
        public int numOfViews { get; set; }

        [DisplayName("Genre")]
        public Genre genre { get; set; }

        virtual public Artist artist { get; set; }
        virtual public Album album { get; set; }
    }
}
