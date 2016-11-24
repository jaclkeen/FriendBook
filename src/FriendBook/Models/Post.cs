using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        [Required]
        public DateTime TimePosted { get; set; }

        public ICollection<User> Likes { get; set; }

        public ICollection<User> Dislikes { get; set; }

        public string ImgUrl { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
