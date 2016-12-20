using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public string ConversationRoomName { get; set; }
        public Conversation Conversation { get; set; }

        [Required]
        public int SendingUserId { get; set; }
        public User SendingUser { get; set; }

        [Required]
        public string MessageText { get; set; }

        [Required]
        public DateTime MessageSentDate { get; set; }
    }
}
