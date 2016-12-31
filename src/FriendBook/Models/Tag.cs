using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public int PersonBeingTaggedId { get; set; }
        public User PersonBeingTagged { get; set; }

        [Required]
        public int TaggerId { get; set; }
        public User Tagger { get; set; }
    }
}
