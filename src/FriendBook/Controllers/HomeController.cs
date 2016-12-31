using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Data;
using FriendBook.ViewModels;
using FriendBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FriendBook.Controllers
{
    public class HomeController : Controller
    {
        private FriendBookContext context;

        //Purpose: To initialize an IHostingEnvironment variable to gain access to the "images" folder when saving
                    //a new image.
        private IHostingEnvironment _environment;

        public HomeController(FriendBookContext ctx, IHostingEnvironment environment)
        {
            _environment = environment;
            context = ctx;
        }


        /**
         * Purpose: Creates an Index view page for the Home controller and sets properties of the HomePageViewModel
         * Arguments:
         *      None
         * Return:
         *      returns the view for the /Home
         */
        public IActionResult Index()
        {
            int UserId = ActiveUser.Instance.User.UserId;

            var relationships = context.Relationship.Where(r => r.ReciverUserId == UserId || r.SenderUserId == UserId).ToList();
            var styling = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();

            var users = context.User.ToList();
            var currentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();

            HomePageViewModel model = new HomePageViewModel(context);
            model.Posts = new List<Post> { };

            List<Post> UserPost = new List<Post>();

            foreach (Relationship r in relationships)
            {
                if (r.ReciverUserId == UserId && r.Status == 1)
                {
                    UserPost = context.Post.Where(p => p.UserId == r.SenderUserId).ToList();
                    if (UserPost != null)
                    {
                        UserPost.ForEach(up => up.User = context.User.Where(user => user.UserId == up.UserId).SingleOrDefault());
                        UserPost.ForEach(up => model.Posts.Add(up));
                    }
                }
                else if (r.SenderUserId == UserId && r.Status == 1)
                {
                    UserPost = context.Post.Where(p => p.UserId == r.ReciverUserId).ToList();

                    if (UserPost != null)
                    {
                        UserPost.ForEach(up => up.User = context.User.Where(user => user.UserId == up.UserId).SingleOrDefault());
                        UserPost.ForEach(up => model.Posts.Add(up));
                    }
                }
            }

            List<Post> UserPosts = context.Post.Where(p => p.UserId == UserId).ToList();
            if (UserPosts != null) { UserPosts.ForEach(up => model.Posts.Add(up)); }

            if (model.Posts.Count == 0)
            {
                model.Posts.Add(new Post
                {
                    Text = "Welcome to FriendBook!",
                    User = context.User.Where(u => u.UserId == 1).SingleOrDefault(),
                    UserId = 1
                });
            }

            model.Posts.ForEach(p => p.Comments = context.Comment.Where(c => c.PostId == p.PostId).ToList());
            model.Posts = model.Posts.OrderByDescending(p => p.TimePosted).ToList();
            model.Posts.ForEach(p => p.TaggedUsers = context.Tag.Where(t => t.PostId == p.PostId).ToList());

            model.UserStyle = styling;
            model.CurrentUserStyle = styling;
            model.CurrentUser = currentUser;

            return View(model);
        }

        /**
        * Purpose: Used to Post a new status with or without an image
        * Arguments:
        *      HomePageViewModel model - Used to pass in all neccessary properties of a post
        * Return:
        *      redirects to the /Home view
        */
        public async Task<IActionResult> NewStatus(HomePageViewModel model)
        {   
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            User u = ActiveUser.Instance.User;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.Post.UserId = u.UserId;
            model.Post.TimePosted = DateTime.Now;

            if (model.PostImgUpload != null && model.PostImgUpload.ContentType.Contains("image"))
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, model.PostImgUpload.FileName), FileMode.Create))
                {
                    await model.PostImgUpload.CopyToAsync(fileStream);
                    model.Post.ImgUrl = $"/images/{model.PostImgUpload.FileName}";
                    context.SaveChanges();
                }
            }

            context.Post.Add(model.Post);
            try
            {
                context.SaveChanges();
            }
            catch
            {
                throw;
            }

            if (model.TaggedUsers != null)
            {
                string[] TaggedUsers = model.TaggedUsers.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                foreach(string id in TaggedUsers)
                {
                    Tag NewTag = new Tag
                    {
                        PostId = model.Post.PostId,
                        PersonBeingTaggedId = Convert.ToInt16(id),
                        TaggerId = u.UserId
                    };

                    Notification NewNotification = new Notification
                    {
                        NotificationText = $"{u.FirstName} {u.LastName} tagged you in a post!",
                        NotificatonDate = DateTime.Now,
                        RecievingUserId = Convert.ToInt16(id),
                        SenderUserId = u.UserId,
                        PostId = model.Post.PostId,
                        Seen = false,
                        NotificationType = "Tag"
                    };

                    context.Notification.Add(NewNotification);
                    context.Tag.Add(NewTag);
                }

                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /**
        * Purpose: HttpGet Method to be called in JavaScript to get a particular user's friends.
        * Arguments:
        *      None
        * Return:
        *      returns a list of the current user's friends
        */
        [HttpGet]
        public List<User> UserFriends()
        {
            int CurrentUserId = ActiveUser.Instance.User.UserId;
            List<User> CurrentUserFriends = new List<User> { };
            List<Relationship> UserRelationships = 
                (from r in context.Relationship
                where r.ReciverUserId == CurrentUserId && r.Status == 1 || r.SenderUserId == CurrentUserId && r.Status == 1
                select r).ToList();

            foreach(Relationship r in UserRelationships)
            {
                if(r.SenderUserId == CurrentUserId)
                {
                    CurrentUserFriends.Add(context.User.Where(u => u.UserId == r.ReciverUserId).SingleOrDefault());
                }

                else
                {
                    CurrentUserFriends.Add(context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault());
                }
            }

            return CurrentUserFriends;
        }

        /**
        * Purpose: HttpGet Method to be called in JavaScript to get the current user.
        * Arguments:
        *      None
        * Return:
        *      returns the current user
        */
        [HttpGet]
        public User GetCurrentUser()
        {
            int UserId = ActiveUser.Instance.User.UserId;

            User CurrentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();
            return CurrentUser;
        }


        /**
        * Purpose: HttpGet Method to be called in JavaScript to get all users for search feature
        * Arguments:
        *      None
        * Return:
        *      returns a list of all users
        */
        [HttpGet]
        public List<User> GetUsers()
        {
            var users = context.User.ToList();
            return users;
        }

        /**
        * Purpose: HttpPost method that is called in JavaScript to accept a friend request (change R status to 1)
        * Arguments:
        *      RelationshipId
        * Return:
        *      None
        */
        [HttpPost]
        public void AcceptFR([FromBody] int id)
        {
            Relationship relationship = context.Relationship.Where(r => r.RelationshipId == id).SingleOrDefault();
            relationship.Status = 1;
            relationship.ReceivingUser = context.User.Where(u => relationship.ReciverUserId == u.UserId).SingleOrDefault();
            relationship.SenderUser = context.User.Where(u => relationship.SenderUserId == u.UserId).SingleOrDefault();

            Notification NewNotification = new Notification
            {
                NotificationText = $"{relationship.ReceivingUser.FirstName} {relationship.ReceivingUser.LastName}, accepted your friend request!",
                NotificatonDate = DateTime.Now,
                RecievingUserId = relationship.SenderUserId,
                SenderUserId = relationship.ReciverUserId,
                Seen = false,
                NotificationType = "FR"
            };

            context.Notification.Add(NewNotification);
            context.SaveChanges();
        }

        /**
        * Purpose: HttpPost method that is called in JavaScript to decline a friend request (change R status to 2)
        * Arguments:
        *      RelationshipId
        * Return:
        *      None
        */
        [HttpPost]
        public void DeclineFR([FromBody] int id)
        {
            Relationship relationship = context.Relationship.Where(r => r.RelationshipId == id).SingleOrDefault();
            relationship.Status = 2;
            context.SaveChanges();
        }

        [HttpGet]
        public List<Notification> UserNotifications()
        {
            List<Notification> notifications = context.Notification.Where(n => n.RecievingUserId == ActiveUser.Instance.User.UserId && n.Seen == false).ToList();
            notifications.ForEach(n => n.RecievingUser = context.User.Where(u => u.UserId == n.RecievingUserId).SingleOrDefault());
            notifications.ForEach(n => n.SendingUser = context.User.Where(u => u.UserId == n.SenderUserId).SingleOrDefault());

            return notifications;
        }
    }
}
