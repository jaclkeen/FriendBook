using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Models;
using FriendBook.Data;

namespace FriendBook.ViewModels
{
    public class ProfileStylingViewModel : ProfileBaseViewModel
    {
        public ProfileStylingViewModel(FriendBookContext ctx, int UserId) : base(ctx, UserId) { }
    }
}
