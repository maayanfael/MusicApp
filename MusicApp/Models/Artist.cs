using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicApp.Models
{
    public class Artist : DbContext
    {
        [Key]
        [DisplayName("Artist Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string firstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string lastName { get; set; }

        [DisplayName("Picture")]
        public string picture { get; set; }

        [DisplayName("Biography")]
        public string biography { get; set; }

        [DisplayName("Number Of views")]
        public int numOfViews { get; set; }

        [DisplayName("Discography")]
        public virtual List<Album> Discography { get; set; }
    }
}
