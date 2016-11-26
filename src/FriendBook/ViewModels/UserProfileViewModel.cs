using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Models;

namespace FriendBook.ViewModels
{
    public class UserProfileViewModel
    {
        public List<User> Friends { get; set; }
        public List<Post> Posts { get; set; }
        public User CurrentUser { get; set; }
        public User UserProfile { get; set; }
        public Style UserStyle { get; set; }
        public Style CurrentUserStyle { get; set; }
        public string AreFriends { get; set; }
    }
}
