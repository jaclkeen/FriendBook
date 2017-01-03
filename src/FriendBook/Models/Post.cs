using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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

        [Required]
        public string PostType { get; set; }
        ////Status, WallPost

        public int? RecievingUserId { get; set; }
        public User RecievingUser { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public List<Tag> TaggedUsers { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string ImgUrl { get; set; }
    }
}
