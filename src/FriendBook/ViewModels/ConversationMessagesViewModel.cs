using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Data;
using FriendBook.Models;

namespace FriendBook.ViewModels
{
    public class ConversationMessagesViewModel : BaseViewModel
    {
        public ConversationMessagesViewModel(FriendBookContext ctx) : base(ctx) { }

        public Style UserStyle { get; set; }

        public Conversation CurrentConversation { get; set; }

        public List<Message> Messages { get; set; }
    }
}
