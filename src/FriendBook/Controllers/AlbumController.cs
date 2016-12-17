using FriendBook.Data;
using FriendBook.Models;
using FriendBook.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Controllers
{
    public class AlbumController : Controller
    {
        private FriendBookContext context;
        private IHostingEnvironment _environment;

        public AlbumController(FriendBookContext ctx, IHostingEnvironment environment)
        {
            _environment = environment;
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

        public async Task<IActionResult> AddImageToAlbum(ProfileIndexViewModel model)
        {
            IFormFile file = model.image;
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            User u = ActiveUser.Instance.User;
            Album selectedAlbum = context.Album.Where(a => a.AlbumId == model.AlbumId).SingleOrDefault();

            if (file != null && file.ContentType.Contains("image"))
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                Image UploadedImage = new Image
                {
                    ImagePath = $"/images/{file.FileName}",
                    UserId = u.UserId,
                    AlbumId = model.AlbumId
                };

                Post NewImagePost = new Post
                {
                    UserId = u.UserId,
                    Text = $"{u.FirstName} added a new photo to their {selectedAlbum.AlbumName} album!",
                    TimePosted = DateTime.Now,
                    ImgUrl = $"/images/{file.FileName}"
                };

                context.Image.Add(UploadedImage);
                context.Post.Add(NewImagePost);

                await context.SaveChangesAsync();
                return RedirectToAction("Albums", "Profile", new { id = u.UserId });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
