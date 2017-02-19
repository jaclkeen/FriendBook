using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Data;
using FriendBook.Models;

namespace FriendBook.ViewModels
{
    public class ConversationIndexViewModel : BaseViewModel
    {
        public ConversationIndexViewModel(FriendBookContext ctx) : base(ctx) { }

        public Style UserStyle { get; set; }

        public List<Conversation> Conversations { get; set; }
    }
}
