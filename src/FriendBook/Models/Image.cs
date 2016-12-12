using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public int AlbumId { get; set; }
        public Album album { get; set; }

        [Required]
        public int UserId { get; set; }
        public User ImageUser { get; set; }

        public string ImageDescription { get; set; }
    }
}
