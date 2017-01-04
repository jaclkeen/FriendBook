using FriendBook.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ProfileImg { get; set; }

        public string CoverImg { get; set; }

        public List<Album> UserAlbums { get; set; }

        public List<Image> UserImages { get; set; }

        public User()
        {
            //this.ProfileImg = "/images/egg.png";
            this.CoverImg = "/images/mountains.jpg";
        }
    }
}
