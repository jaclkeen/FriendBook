using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Data;
using FriendBook.Models;

namespace FriendBook.ViewModels
{
    public class ProfileFriendsViewModel : ProfileBaseViewModel
    {
        public ProfileFriendsViewModel(FriendBookContext ctx, int UserProfileId) : base(ctx, UserProfileId) { }
        public List<User> Friends { get; set; }
    }
}
