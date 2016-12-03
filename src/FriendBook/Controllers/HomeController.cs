﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Data;
using FriendBook.ViewModels;
using FriendBook.Models;

namespace FriendBook.Controllers
{
    public class HomeController : Controller
    {
        private FriendBookContext context;

        public HomeController(FriendBookContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            int UserId = ActiveUser.Instance.User.UserId;
            //REPLACE WITH REAL CURRENT USER WHEN LOGIN IS CREATED
            var relationships = context.Relationship.Where(r => r.ReciverUserId == UserId || r.SenderUserId == UserId).ToList();
            var styling = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();
            //var posts = context.Post.OrderByDescending(p => p.TimePosted).ToList();
            var users = context.User.ToList();
            //REPLACE WITH REAL CURRENT USER WHEN LOGIN IS CREATED
            var currentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();

            HomePageViewModel model = new HomePageViewModel(context);
            model.Posts = new List<Post> { };

            foreach (Relationship r in relationships)
            {
                Post UserPost;
                //REPLACE WITH REAL USER WHEN LOGIN IS AVAILABLE
                if(r.ReciverUserId == UserId && r.Status == UserId)
                {
                    UserPost = context.Post.Where(p => p.UserId == r.SenderUserId).SingleOrDefault();
                    if (UserPost != null)
                    {
                        UserPost.User = context.User.Where(u => u.UserId == UserPost.UserId).SingleOrDefault();
                        model.Posts.Add(UserPost);
                    }
                }
                //REPLACE WITH REAL USER WHEN LOGIN IS AVAILABLE
                else if (r.SenderUserId == UserId && r.Status == 1)
                {
                    UserPost = context.Post.Where(p => p.UserId == r.ReciverUserId).SingleOrDefault();
                    if (UserPost != null)
                    {
                        UserPost.User = context.User.Where(u => u.UserId == UserPost.UserId).SingleOrDefault();
                        model.Posts.Add(UserPost);
                    }
                }
            }

            //REPLACE WITH REAL USER WHEN LOGIN IS AVAILABLE
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
            model.Posts.OrderBy(p => p.TimePosted);
            model.UserStyle = styling;
            model.CurrentUserStyle = styling;
            model.CurrentUser = currentUser;

            return View(model);
        }

        public IActionResult NewStatus(Post post)
        {
            int UserId = ActiveUser.Instance.User.UserId;
            //REPLACE WITH REAL CURRENT USER WHEN LOGIN IS CREATED
            post.UserId = UserId;
            post.TimePosted = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Post.Add(post);
            try
            {
                context.SaveChanges();
            }
            catch
            {
                throw;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public User GetCurrentUser()
        {
            int UserId = ActiveUser.Instance.User.UserId;
            //REPLACE WITH REAL USER WHEN LOGIN IS CREATED
            User CurrentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();
            return CurrentUser;
        }

        [HttpGet]
        public List<User> GetUsers()
        {
            var users = context.User.ToList();
            return users;
        }

        [HttpPost]
        public void AcceptFR([FromBody] int id)
        {
            Relationship relationship = context.Relationship.Where(r => r.RelationshipId == id).SingleOrDefault();
            relationship.Status = 1;
            context.SaveChanges();
        }

        [HttpPost]
        public void DeclineFR([FromBody] int id)
        {
            Relationship relationship = context.Relationship.Where(r => r.RelationshipId == id).SingleOrDefault();
            relationship.Status = 2;
            context.SaveChanges();
        }
    }
}
