using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Data;
using FriendBook.Models;
using Microsoft.AspNetCore.Http;

namespace FriendBook.ViewModels
{
    public class ProfileAlbumViewModel : ProfileBaseViewModel
    {
        public ProfileAlbumViewModel(FriendBookContext ctx, int UserProfileId) : base(ctx, UserProfileId) { }

        public List<Album> UserAlbums { get; set; }

        public Album NewAlbum { get; set; }
    }
}
