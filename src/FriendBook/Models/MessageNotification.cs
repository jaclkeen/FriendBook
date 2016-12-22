using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class MessageNotification
    {
        [Key]
        public int MessageNotificationId { get; set; }

        [Required]
        public int SendingUserId { get; set; }
        public User SendingUser { get; set; }

        [Required]
        public int RecievingUserId { get; set; }
        public User RecievingUser { get; set; }

        [Required]
        public bool Seen { get; set; }

        public MessageNotification()
        {
            this.Seen = false;
        }
    }
}
