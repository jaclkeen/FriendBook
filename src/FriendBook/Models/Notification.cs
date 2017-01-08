using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string NotificationText { get; set; }

        [Required]
        public int SenderUserId { get; set; }
        public User SendingUser { get; set; }

        [Required]
        public int RecievingUserId { get; set; }
        public User RecievingUser { get; set; }

        [Required]
        public DateTime NotificatonDate { get; set; }

        [Required]
        public bool Seen { get; set; }

        [Required]
        public string NotificationType { get; set; }
        //Types: FR, LikeDislikeOrComment, Tag, Sale

        public int PostId { get; set; }

        public int YardSaleItemId { get; set; }
    }
}
