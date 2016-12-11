using FriendBook.Data;
using FriendBook.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Controllers
{
    public class AlbumController : Controller
    {
        private FriendBookContext context;

        public AlbumController(FriendBookContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public void CreateNewAlbum([FromBody] Album album)
        {
            User CurrentUser = ActiveUser.Instance.User;
            album.UserId = CurrentUser.UserId;

            context.Album.Add(album);
            context.SaveChanges();
        }
    }
}
