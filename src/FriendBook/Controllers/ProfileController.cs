﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.ViewModels;
using FriendBook.Data;
using FriendBook.Models;
using System.IO;

namespace FriendBook.Controllers
{
    public class ProfileController : Controller
    {
        private FriendBookContext context;

        public ProfileController(FriendBookContext ctx)
        {
            context = ctx;
        }

        public IActionResult Profile([FromRoute] int id)
        {
            int UserId = ActiveUser.Instance.User.UserId;

            User user = context.User.Where(u => u.UserId == id).SingleOrDefault();
            Style style = context.Style.Where(s => s.UserId == id).SingleOrDefault();
            List<Post> posts = context.Post.Where(p => p.UserId == id).ToList();
            //LATER REPLACE WITH CURRENT USER
            List<Relationship> relationships = context.Relationship.Where(r => r.ReciverUserId == UserId || r.SenderUserId == UserId).ToList();
            UserProfileViewModel model = new UserProfileViewModel(context);
            model.Friends = new List<User> { };

            foreach(Relationship r in relationships)
            {
                User friend;
                if(r.ReciverUserId == id && r.Status == 1)
                {
                    friend = context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault();
                    model.Friends.Add(friend);
                }
                else if(r.SenderUserId == id && r.Status == 1)
                {
                    friend = context.User.Where(u => u.UserId == r.ReciverUserId).SingleOrDefault();
                    model.Friends.Add(friend);
                }
            }
            
            model.AreFriends = "NoRelationship";
            foreach(Relationship r in relationships)
            {
                if(r.SenderUserId == id || r.ReciverUserId == id)
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

            posts.ForEach(p => p.Comments = context.Comment.Where(c => c.PostId == p.PostId).ToList());
            
            model.CurrentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();
            model.CurrentUserStyle = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();
            model.UserProfile = user;
            model.UserStyle = style;
            model.Posts = posts;

            return View(model);
        }

        public IActionResult AddFriend([FromRoute] int id)
        {
            int UserId = ActiveUser.Instance.User.UserId;
            //REPLACE WITH CURRENT USER LATER
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
            //LATER REPLACE WITH CURRENT USER
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

            context.SaveChanges();
            return RedirectToAction("Profile", "Profile", new { id });
        }
    }
}
