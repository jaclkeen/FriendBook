using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Models;
using FriendBook.Data;

namespace FriendBook.ViewModels
{
    public class UserStylingViewModel : BaseViewModel
    {
        public UserStylingViewModel(FriendBookContext ctx) : base(ctx) { }

        public Style UserStyle { get; set; }
    }
}
