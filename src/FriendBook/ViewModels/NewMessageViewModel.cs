using FriendBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.ViewModels
{
    public class NewMessageViewModel
    {
        public Message message { get; set; }

        public string ConversationRoomName { get; set; }
    }
}
