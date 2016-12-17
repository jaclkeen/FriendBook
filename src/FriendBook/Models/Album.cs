using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        public int UserId { get; set; }
        public User AlbumUser { get; set; }

        public string AlbumName { get; set; }

        public string AlbumDescription { get; set; }

        public List<Image> AlbumImages { get; set; }
    }
}
