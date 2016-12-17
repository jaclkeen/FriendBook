using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Models;
using FriendBook.Data;
using Microsoft.AspNetCore.Http;

namespace FriendBook.ViewModels
{
    public class UserProfileViewModel : ProfileBaseViewModel
    {
        public UserProfileViewModel(FriendBookContext ctx, int id) : base(ctx, id) { }
        public UserProfileViewModel() { }
        public List<User> Friends { get; set; }
        public List<Post> Posts { get; set; }
        public List<Album> UserAlbums { get; set; }
        public Album NewAlbum { get; set; }
        public int AlbumId { get; set; }
        public IFormFile image { get; set; }
    }
}
