using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Models;
using FriendBook.Data;

namespace FriendBook.ViewModels
{
    public class HomePageViewModel
    {
        public List<Relationship> FriendRequests{ get; set; }
        public User CurrentUser { get; set; }
        public List<Post> Posts { get; set; }
        public List<User> Users { get; set; }
        public Post Post { get; set; }
        public Style CurrentUserStyle { get; set; }
        public Style UserStyle { get; set; }
    }
}
