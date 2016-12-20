using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Conversation
    {
        [Key]
        public string ConversationRoomName { get; set; }

        [Required]
        public int ConversationStarterId { get; set; }
        public User ConversationStarter { get; set; }

        [Required]
        public int ConversationRecieverId { get; set; }
        public User ConversationReciever { get; set; }

        public List<Message> ConversationMessages { get; set; }
    }
}
