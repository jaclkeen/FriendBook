using FriendBook.Data;
using FriendBook.Models;
using FriendBook.ViewModels;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public Album GetSpecificAlbum([FromRoute] int id)
        {
            Album SelectedAlbum = context.Album.Where(a => a.AlbumId == id).SingleOrDefault();
            return SelectedAlbum;
        }

        [HttpGet]
        public List<Image> GetAlbumImages([FromRoute] int id)
        {
            List<Image> FoundImages = context.Image.Where(i => i.AlbumId == id).ToList();
            return FoundImages;
        }

        public IActionResult AddImageToAlbum(UserProfileViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
