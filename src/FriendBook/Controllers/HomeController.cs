﻿using System;
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

            List<Relationship> relationships = context.Relationship.Where(r => r.ReciverUserId == UserId || r.SenderUserId == UserId).ToList();
            Style styling = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();

            List<User> users = context.User.ToList();
            User currentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();

            HomePageViewModel model = new HomePageViewModel(context);
            model.Posts = new List<Post> { };

            List<Post> UserPost = new List<Post>();

            foreach (Relationship r in relationships)
            {
                if (r.ReciverUserId == UserId && r.Status == 1)
                {
                    UserPost = context.Post.Where(p => p.UserId == r.SenderUserId).ToList();

                    List<Post> ProfileUserTagPosts = (from t in context.Tag
                                                      join p in context.Post on t.PostId equals p.PostId
                                                      where t.PersonBeingTaggedId == r.SenderUserId
                                                      select p).ToList();

                    ProfileUserTagPosts.ForEach(TaggedPost => { if (UserPost.Contains(TaggedPost)) { UserPost.Add(TaggedPost); } });

                    if (UserPost != null)
                    {
                        UserPost.ForEach(up => up.User = context.User.Where(user => user.UserId == up.UserId).SingleOrDefault());
                        UserPost.ForEach(up => model.Posts.Add(up));
                    }
                }
                else if (r.SenderUserId == UserId && r.Status == 1)
                {
                    UserPost = context.Post.Where(p => p.UserId == r.ReciverUserId).ToList();

                    List<Post> ProfileUserTagPosts = (from t in context.Tag
                                                      join p in context.Post on t.PostId equals p.PostId
                                                      where t.PersonBeingTaggedId == r.ReciverUserId
                                                      select p).ToList();

                    ProfileUserTagPosts.ForEach(TaggedPost => { if (UserPost.Contains(TaggedPost)) { UserPost.Add(TaggedPost); } });

                    if (UserPost != null)
                    {
                        UserPost.ForEach(up => up.User = context.User.Where(user => user.UserId == up.UserId).SingleOrDefault());
                        UserPost.ForEach(up => model.Posts.Add(up));
                    }
                }
            }

            //GETS ALL OF THE CURRENTS USER'S POSTS AND MENTIONS
            List<Post> UserPosts = context.Post.Where(p => p.UserId == UserId || p.RecievingUserId == UserId).ToList();

            //IF POSTS AND MENTIONS != NULL AND THE POSTS DON'T ALREADY INCLUDE THE WALLPOST OF THE RECIEVING USER ADD TO TOTAL POST LIST
            if (UserPosts != null) {
                UserPosts.ForEach(p => { if (p.RecievingUserId != null) { p.RecievingUser = context.User.Where(u => u.UserId == p.RecievingUserId).SingleOrDefault(); } });

                model.Posts = model.Posts.Union(UserPosts).ToList();
            }

            if (model.Posts.Count == 0)
            {
                model.Posts.Add(context.Post.Where(p => p.PostId == 0).SingleOrDefault());
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

            model.Post.PostType = "Status";
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

            CurrentUserFriends = CurrentUserFriends.OrderBy(f => f.FirstName + f.LastName).ToList();

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
            //USED TO PREVENT ERROR ON LOGIN PAGE WHEN ACTIVEUSER IS NULL
            if(ActiveUser.Instance.User == null)
            {
                return null;
            }

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

        /**
        * Purpose: HttpGet method that returns all of the current user's notifications
        * Arguments:
        *      None
        * Return:
        *      A list of all the current user's notifications
        */
        [HttpGet]
        public List<Notification> UserNotifications()
        {
            List<Notification> notifications = context.Notification.Where(n => n.RecievingUserId == ActiveUser.Instance.User.UserId && n.Seen == false).OrderByDescending(d => d.NotificatonDate).ToList();
            notifications.ForEach(n => n.RecievingUser = context.User.Where(u => u.UserId == n.RecievingUserId).SingleOrDefault());
            notifications.ForEach(n => n.SendingUser = context.User.Where(u => u.UserId == n.SenderUserId).SingleOrDefault());


            return notifications;
        }

        /**
        * Purpose: HttpPost method that is called to update that a notification has been seen
        * Arguments:
        *      RelationshipId
        * Return:
        *      None
        */
        [HttpPost]
        public void SeenUserNotification([FromBody] int NotificationId)
        {
            Notification notification = context.Notification.Where(n => n.NotificationId == NotificationId).SingleOrDefault();
            notification.Seen = true;

            context.SaveChanges();
        }

        /**
        * Purpose: HttpGet method that gets all of the sent to him/her current user's pending friend requests
        * Arguments:
        *      None
        * Return:
        *      A list of all the current user's recieved pending friend requests
        */
        [HttpGet]
        public List<Relationship> UserFriendRequests()
        {
            User CurrentUser = ActiveUser.Instance.User;
            List<Relationship> requests = context.Relationship.Where(r => r.ReciverUserId == CurrentUser.UserId && r.Status == 0).ToList();
            requests.ForEach(r => r.SenderUser = context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault());


            return requests;
        }
    }
}
