using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Data;
using FriendBook.Models;
using Microsoft.AspNetCore.Http;

namespace FriendBook.ViewModels
{
    public class ProfileAlbumImagesViewModel : ProfileBaseViewModel
    {
        public ProfileAlbumImagesViewModel(FriendBookContext ctx, int UserProfileId) : base(ctx, UserProfileId) { }

        public Album ChosenAlbum { get; set; }

        public List<Image> AlbumImages { get; set; }

        public int AlbumId { get; set; }

        public IFormFile image { get; set; }
    }
}
