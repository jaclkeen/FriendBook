using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.ViewModels;
using FriendBook.Data;
using FriendBook.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Hosting;

namespace FriendBook.Controllers
{
    public class ProfileController : Controller
    {
        private FriendBookContext context;
        private IHostingEnvironment _environment;

        public ProfileController(FriendBookContext ctx, IHostingEnvironment environment)
        {
            _environment = environment;
            context = ctx;
        }

        public IActionResult Profile([FromRoute] int id)
        {
            int UserId = ActiveUser.Instance.User.UserId;

            User user = context.User.Where(u => u.UserId == id).SingleOrDefault();
            Style style = context.Style.Where(s => s.UserId == id).SingleOrDefault();
            List<Post> posts = context.Post.Where(p => p.UserId == id).ToList();

            List<Relationship> relationships = context.Relationship.Where(r => r.ReciverUserId == id || r.SenderUserId == id).ToList();
            UserProfileViewModel model = new UserProfileViewModel(context);
            model.Friends = new List<User> { };

            foreach (Relationship r in relationships)
            {
                User friend;
                if (r.ReciverUserId == id && r.Status == 1)
                {
                    friend = context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault();
                    model.Friends.Add(friend);
                }
                else if (r.SenderUserId == id && r.Status == 1)
                {
                    friend = context.User.Where(u => u.UserId == r.ReciverUserId).SingleOrDefault();
                    model.Friends.Add(friend);
                }
            }

            if (UserId != id)
            {
                foreach (Relationship r in relationships)
                {
                    if (r.SenderUserId == UserId || r.ReciverUserId == UserId)
                    {
                        if (r.Status == 0)
                        {
                            model.AreFriends = "Pending";
                            break;
                        }
                        else if (r.Status == 1)
                        {
                            model.AreFriends = "yes";
                            break;
                        }
                        else if (r.Status == 2)
                        {
                            model.AreFriends = "no";
                            break;
                        }
                        else
                        {
                            model.AreFriends = "blocked";
                        }
                    }
                }

                if(model.AreFriends == null)
                {
                    model.AreFriends = "NoRelationship";
                }
            }

            posts.ForEach(p => p.Comments = context.Comment.Where(c => c.PostId == p.PostId).ToList());
            foreach (Post p in posts)
            {
                if (p.Comments != null)
                {
                    foreach (Comment c in p.Comments)
                    {
                        c.User = context.User.Where(u => u.UserId == c.UserId).SingleOrDefault();
                    }
                }
            }

            //mind blowing magic going on here
            List<Album> albums = context.Album.Where(a => a.UserId == id).ToList();
            List<Image> images = context.Image.Where(i => i.UserId == id).ToList();

            model.UserAlbums = albums;
            model.Friends.OrderBy(f => f.FirstName);
            model.CurrentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();
            model.CurrentUserStyle = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();
            model.UserProfile = user;
            model.Posts = posts;
            model.UserStyle = style;

            return View(model);
        }

        public IActionResult AddFriend([FromRoute] int id)
        {
            int UserId = ActiveUser.Instance.User.UserId;

            User currentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();
            User userBeingAdded = context.User.Where(u => u.UserId == id).SingleOrDefault();

            Relationship ABeautifulRelationship = new Relationship
            {
                SenderUserId = currentUser.UserId,
                SenderUser = context.User.Where(u => u.UserId == currentUser.UserId).SingleOrDefault(),
                ReciverUserId = userBeingAdded.UserId,
                ReceivingUser = context.User.Where(u => u.UserId == userBeingAdded.UserId).SingleOrDefault(),
                Status = 0
            };

            context.Relationship.Add(ABeautifulRelationship);
            try
            {
                context.SaveChanges();
            }
            catch
            {
                throw;
            }

            return RedirectToAction("Profile", new { id });
        }

        public IActionResult Styling(int id)
        {
            int UserId = ActiveUser.Instance.User.UserId;

            UserStylingViewModel model = new UserStylingViewModel(context);
            model.UserStyle = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();

            return View(model);
        }

        public IActionResult UpdateUserStyling([FromRoute] int id, Style UserStyle)
        {
            Style style = context.Style.Where(s => s.UserId == id).SingleOrDefault();

            style.BackgroundColor = UserStyle.BackgroundColor;
            style.DetailColor = UserStyle.DetailColor;
            style.FontColor = UserStyle.FontColor;
            style.FontFamily = UserStyle.FontFamily;
            style.FontSize = UserStyle.FontSize;
            style.NavColor = UserStyle.NavColor;
            style.WallBackgroundColor = UserStyle.WallBackgroundColor;
            style.PostBackgroundColor = UserStyle.PostBackgroundColor;
            style.PostHeaderColor = UserStyle.PostHeaderColor;

            context.SaveChanges();
            return RedirectToAction("Profile", "Profile", new { id });
        }

        public async Task<IActionResult> UploadImg(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            User u = ActiveUser.Instance.User;
            User CurrentDbUser = context.User.Where(us => us.UserId == u.UserId).SingleOrDefault();
            Album UserUploadAlbum = context.Album.Where(a => a.UserId == u.UserId && a.AlbumName == "Uploads").SingleOrDefault();

            if (file != null && file.ContentType.Contains("image"))
            {
                if(UserUploadAlbum == null)
                {
                    UserUploadAlbum = new Album
                    {
                        UserId = u.UserId,
                        AlbumName = "Uploads",
                        AlbumDescription = "Album used for all uploads!"
                    };
                    context.Album.Add(UserUploadAlbum);
                    context.SaveChanges();
                }

                Image UploadedImage = new Image
                {
                    ImagePath = $"/images/{file.FileName}",
                    UserId = u.UserId,
                    AlbumId = UserUploadAlbum.AlbumId
                };
                context.Image.Add(UploadedImage);

                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    CurrentDbUser.ProfileImg = $"/images/{file.FileName}";
                    context.SaveChanges();
                }
                    return RedirectToAction("Profile", "Profile", new { id = u.UserId });
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UploadCoverImg(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            User u = ActiveUser.Instance.User;
            User CurrentDbUser = context.User.Where(us => us.UserId == u.UserId).SingleOrDefault();

            if (file != null && file.ContentType.Contains("image"))
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    CurrentDbUser.CoverImg = $"/images/{file.FileName}";
                    context.SaveChanges();
                }
                return RedirectToAction("Profile", "Profile", new { id = u.UserId });
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
