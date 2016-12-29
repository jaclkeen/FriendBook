using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Models;
using FriendBook.Data;
using Microsoft.AspNetCore.Http;

namespace FriendBook.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel() { }
        public HomePageViewModel(FriendBookContext ctx) : base(ctx) { }
        public List<Post> Posts { get; set; }
        public List<User> Users { get; set; }
        public Post Post { get; set; }
        public Style UserStyle { get; set; }
        public IFormFile PostImgUpload { get; set; }
        public string TaggedUsers { get; set; }
    }
}
