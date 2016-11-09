using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppProj2.Models
{
    public class Artist
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

        [DisplayName("Number Of views")]
        public int numOfViews { get; set; }

        [DisplayName("Songs")]
        public virtual List<Song> songs { get; set; }
    }
}
