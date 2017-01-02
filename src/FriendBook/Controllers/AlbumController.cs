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

        //Purpose: To initialize an IHostingEnvironment variable to gain access to the "images" folder when saving
        //a new image.
        private IHostingEnvironment _environment;

        public AlbumController(FriendBookContext ctx, IHostingEnvironment environment)
        {
            _environment = environment;
            context = ctx;
        }

        /**
        * Purpose: HttpPost method that is called in JavaScript to create a new photo album
        * Arguments:
        *      Album album - contains all neccesary Album properties to create a new album
        * Return:
        *      None
        */
        [HttpPost]
        public void CreateNewAlbum([FromBody] Album album)
        {
            User CurrentUser = ActiveUser.Instance.User;
            album.UserId = CurrentUser.UserId;

            context.Album.Add(album);
            context.SaveChanges();
        }

        /**
        * Purpose: HttpGet method that is called in JavaScript to return a specific album
        * Arguments:
        *      int id - The particualar albumId that is being returned
        * Return:
        *      SelectedAlbum - The album found from the id being passed into the method
        */
        [HttpGet]
        public Album GetSpecificAlbum([FromRoute] int id)
        {
            Album SelectedAlbum = context.Album.Where(a => a.AlbumId == id).SingleOrDefault();
            return SelectedAlbum;
        }

        /**
        * Purpose: HttpGet method that is called in JavaScript to return all images within a particular album
        * Arguments:
        *      int id - The particular album that contains its images
        * Return:
        *      FoundImages - A list of images that have the particular albumId
        */
        [HttpGet]
        public List<Image> GetAlbumImages([FromRoute] int id)
        {
            List<Image> FoundImages = context.Image.Where(i => i.AlbumId == id).ToList();
            return FoundImages;
        }

        /**
        * Purpose: To upload a new image into an album
        * Arguments:
        *      ProfileIndexViewModel model - Contains the albumId selected and all properties neccasary to create a new image
        * Return:
        *      Redirects to the list of albums view.
        */
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
                return RedirectToAction("AlbumImages", "Profile", new { id = u.UserId, id2 = model.AlbumId});
            }

            return RedirectToAction("Index", "Home");
        }

        /**
        * Purpose: This method is used to delete a single image from a photo album
        * Arguments:
        *      [FromRoute] int id - The id of the particular image being deleted
        * Return:
        *      Redirects to the user back to the current album they are viewing
        */
        public IActionResult DeleteImageFromAlbum([FromRoute] int id)
        {
            Image DeletedImage = context.Image.Where(i => i.ImageId == id).SingleOrDefault();
            context.Remove(DeletedImage);
            context.SaveChanges();

            return RedirectToAction("AlbumImages", "Profile", new { id = ActiveUser.Instance.User.UserId, id2 = DeletedImage.AlbumId });
        }

        /**
        * Purpose: This method is used to delete an entire album along with the images in that album
        * Arguments:
        *      [FromRoute] int id - The id of the particular album being deleted
        * Return:
        *      Redirects to the user back to the list of user albums page
        */
        public IActionResult DeleteAlbum([FromRoute] int id)
        {
            Album DeletedAlbum = context.Album.Where(a => a.AlbumId == id).SingleOrDefault();
            context.Album.Remove(DeletedAlbum);

            List<Image> ImagesInAlbum = context.Image.Where(i => i.AlbumId == id).ToList();
            ImagesInAlbum.ForEach(image => context.Image.Remove(image));

            context.SaveChanges();

            return RedirectToAction("Albums", "Profile", new { id = ActiveUser.Instance.User.UserId, id2 = id });
        }
    }
}
