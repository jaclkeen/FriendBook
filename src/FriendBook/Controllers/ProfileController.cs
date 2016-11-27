using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.ViewModels;
using FriendBook.Data;
using FriendBook.Models;

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
            User user = context.User.Where(u => u.UserId == id).SingleOrDefault();
            Style style = context.Style.Where(s => s.UserId == id).SingleOrDefault();
            List<Post> posts = context.Post.Where(p => p.UserId == id).ToList();
            //LATER REPLACE WITH CURRENT USER
            List<Relationship> relationships = context.Relationship.Where(r => r.ReciverUserId == 1 || r.SenderUserId == 1).ToList();
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

            //REPLACE WITH REAL CURRENT USER WHEN LOGIN IS CREATED
            model.CurrentUser = context.User.Where(u => u.UserId == 1).SingleOrDefault();
            model.CurrentUserStyle = context.Style.Where(s => s.UserId == 1).SingleOrDefault();

            model.UserProfile = user;
            model.UserStyle = style;
            model.Posts = posts;

            return View(model);
        }

        public IActionResult AddFriend([FromRoute] int id)
        {
            //REPLACE WITH CURRENT USER LATER
            User currentUser = context.User.Where(u => u.UserId == 1).SingleOrDefault();
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

        public IActionResult Styling()
        {
            UserStylingViewModel model = new UserStylingViewModel(context);
            //LATER REPLACE WITH CURRENT USER
            model.UserStyle = context.Style.Where(s => s.UserId == 1).SingleOrDefault();

            return View(model);
        }

        public IActionResult UpdateUserStyling(Style UserStyle)
        {
            return RedirectToAction("Profile", "Profile", new { id = 1 });
        }
    }
}
