using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Models;
using FriendBook.Data;

namespace FriendBook.ViewModels
{
    public class UserStylingViewModel : ProfileBaseViewModel
    {
        public UserStylingViewModel(FriendBookContext ctx, int UserId) : base(ctx, UserId) { }
    }
}
