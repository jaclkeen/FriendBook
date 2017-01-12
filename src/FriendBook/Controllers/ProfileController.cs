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

        //Purpose: To initialize an IHostingEnvironment variable to gain access to the "images" folder when saving
        //a new image.
        private IHostingEnvironment _environment;

        public ProfileController(FriendBookContext ctx, IHostingEnvironment environment)
        {
            _environment = environment;
            context = ctx;
        }

        /**
        * Purpose: Method that is used to return the /Profile view and sets properties of the ProfileIndexViewModel
        * Arguments:
        *      int id - the userId of the profile being visited
        * Return:
        *      the view for the profile index view
        */
        public IActionResult Index([FromRoute] int id)
        {
            User ProfileUser = context.User.Where(u => u.UserId == id).SingleOrDefault();
            User SignedInUser = ActiveUser.Instance.User;
            Notification ProfileSeenNotificationExists = context.Notification.Where(c => c.NotificationType == "ProfileView" && c.RecievingUserId == ProfileUser.UserId && c.SenderUserId == SignedInUser.UserId && c.Seen == false).SingleOrDefault();

            if (ProfileUser.UserId != SignedInUser.UserId && ProfileSeenNotificationExists == null)
            {
                Notification NewNotification = new Notification
                {
                    NotificationText = $"{SignedInUser.FirstName} {SignedInUser.LastName}, viewed your profile page!",
                    NotificationType = "ProfileView",
                    NotificatonDate = DateTime.Now,
                    RecievingUserId = ProfileUser.UserId,
                    Seen = false,
                    SenderUserId = SignedInUser.UserId
                };

                context.Notification.Add(NewNotification);
                context.SaveChanges();
            }

            int UserId = ActiveUser.Instance.User.UserId;

            List<Post> posts = context.Post.Where(p => p.UserId == id || p.RecievingUserId == id).ToList();

            List<Post> ProfileUserTagPosts = (from t in context.Tag
                                              join p in context.Post on t.PostId equals p.PostId
                                              where t.PersonBeingTaggedId == id
                                              select p).ToList();
            ProfileUserTagPosts.ForEach(TaggedPost => posts.Add(TaggedPost));

            posts.ForEach(p => p.User = context.User.Where(u => u.UserId == p.UserId).SingleOrDefault());
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

                if(p.RecievingUserId != null)
                {
                    p.RecievingUser = context.User.Where(u => u.UserId == p.RecievingUserId).SingleOrDefault();
                }
            }

            ProfileIndexViewModel model = new ProfileIndexViewModel(context, id);
            model.Posts = posts.OrderByDescending(p => p.TimePosted).ToList();
            model.Posts.ForEach(p => p.TaggedUsers = context.Tag.Where(t => t.PostId == p.PostId).ToList());
            model.Posts.ForEach(p => p.TaggedUsers.ForEach(u => u.PersonBeingTagged = context.User.Where(us => us.UserId == u.PersonBeingTaggedId).SingleOrDefault()));

            return View(model);
        }

        /**
        * Purpose: Method returns the view that includes all images of a particular album
        * Arguments:
        *       id - that particular profile user's userid
        *       id2 - that id of the album being viewed
        * Return:
        *      The view that contains the images
        */
        public IActionResult AlbumImages(int id, int id2)
        {
            Album album = context.Album.Where(a => a.AlbumId == id2).SingleOrDefault();
            List<Image> ImagesForAlbum = context.Image.Where(i => i.AlbumId == id2).ToList();

            ProfileAlbumImagesViewModel model = new ProfileAlbumImagesViewModel(context, id)
            {
                ChosenAlbum = album,
                AlbumImages = ImagesForAlbum,
                AlbumId = id2
            };

            return View(model);
        }

        /**
        * Purpose: Method that is used to return the /Profile/Friends that shows all of a users friends 
        *           view and sets properties of the ProfileFriendsViewModel
        * Arguments:
        *      int id - the userId of the profile being visited
        * Return:
        *      the view for the Profile/Friends view
        */
        public IActionResult Friends([FromRoute] int id)
        {
            ProfileFriendsViewModel model = new ProfileFriendsViewModel(context, id);

            List<Relationship> relationships = context.Relationship.Where(r => r.ReciverUserId == id || r.SenderUserId == id).ToList();
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

            model.Friends.OrderBy(f => f.FirstName);

            return View(model);
        }

        /**
        * Purpose: Method that is used to return the /Profile/Albums that shows all of a users albums 
        *           view and sets properties of the ProfileAlbumViewModel
        * Arguments:
        *      int id - the userId of the profile being visited
        * Return:
        *      the view for the Profile/Albums view
        */
        public IActionResult Albums([FromRoute] int id)
        {
            ProfileAlbumViewModel model = new ProfileAlbumViewModel(context, id);

            //mind blowing magic going on here
            List<Album> albums = context.Album.Where(a => a.UserId == id).ToList();
            List<Image> images = context.Image.Where(i => i.UserId == id).ToList();

            model.UserAlbums = albums;

            return View(model);
        }

        /**
        * Purpose: Method that is called to create a new relationship between two users
        * Arguments:
        *      int id - the userId of the profile being visited
        * Return:
        *      Redirects to the current userProfile page being visited
        */
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

            return RedirectToAction("Index", "Profile", new { id });
        }

        /**
        * Purpose: Method that is used to return the /Profile/Styling that shows the form to update the 
        *           currentUser's style view and sets properties of the ProfileStylingViewModel
        * Arguments:
        *      int id - the userId of the profile being visited
        * Return:
        *      the view for the profile index view
        */
        public IActionResult Styling(int id)
        {
            User CurrentUser = ActiveUser.Instance.User;

            ProfileStylingViewModel model = new ProfileStylingViewModel(context, id);
            model.UserStyle = context.Style.Where(s => s.UserId == CurrentUser.UserId).SingleOrDefault();

            return View(model);
        }

        /**
        * Purpose: Method that is used to update the current users styling from the properties in the Styling form
        * Arguments:
        *      int id - The currentUser's userId
        *      Style UserStyle - the new style that was created by the current user
        * Return:
        *      the view for the profile index view
        */
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
            return RedirectToAction("Index", "Profile", new { id });
        }

        /**
        * Purpose: Method that is used to change the currentUser's profile Image. Also creates a new "Uploads" album
        *           if one doesn't already exists and adds the newly uploaded image to that album
        * Arguments:
        *      IFormFile file - the image being uploaded
        * Return:
        *      If successful, redirects to the currentUser's /Profile (Index) view
        */
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
                    return RedirectToAction("Index", "Profile", new { id = u.UserId });
            }

            return RedirectToAction("Index", "Home");
        }

        /**
        * Purpose: Method that is used to change the currentUser's cover image.
        * Arguments:
        *      IFormFile file - the image being uploaded
        * Return:
        *      If successful, redirects to the currentUser's /Profile (Index) view
        */
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
                return RedirectToAction("Index", "Profile", new { id = u.UserId });
            }

            return RedirectToAction("Index", "Home");
        }

        /**
        * Purpose: To post a wall post on to another user's wall
        * Arguments:
        *       id - The Userid of the user's profile page being visited
        *       model - contains all of the neccessary properties to create a new WallPost and notification
        * Return:
        *      Redirects to that same user's profile page
        */
        public IActionResult CreateWallPost([FromRoute] int id, ProfileBaseViewModel model)
        {
            User PostingUser = ActiveUser.Instance.User;
            User RecievingUser = context.User.Where(u => u.UserId == id).SingleOrDefault();

            model.WallPost.PostType = "WallPost";
            model.WallPost.RecievingUserId = RecievingUser.UserId;
            model.WallPost.TimePosted = DateTime.Now;
            model.WallPost.UserId = PostingUser.UserId;

            context.Post.Add(model.WallPost);
            context.SaveChanges();

            Notification WallPostNotification = new Notification
            {
                NotificationText = $"{PostingUser.FirstName} {PostingUser.LastName} wrote on your wall!",
                NotificationType = "Tag",
                NotificatonDate = DateTime.Now,
                RecievingUserId = RecievingUser.UserId,
                Seen = false,
                SenderUserId = PostingUser.UserId,
                PostId = model.WallPost.PostId,
            };

            context.Notification.Add(WallPostNotification);
            context.SaveChanges();

            return RedirectToAction("Index", "Profile", new { id = RecievingUser.UserId });
        }

    }
}
