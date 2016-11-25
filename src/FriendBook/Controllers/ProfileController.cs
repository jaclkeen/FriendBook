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
            List<Relationship> relationships = context.Relationship.ToList();

            UserProfileViewModel model = new UserProfileViewModel();
            model.AreFriends = "NoRelationship";
            foreach(Relationship r in relationships)
            {
                if(r.UserId1 == id || r.UserId2 == id && r.UserId1 == 1 || r.UserId2 == 1)
                {
                    if (r.Status == 0)
                    {
                        model.AreFriends = "Pending";
                        break;
                    }
                    if (r.Status == 1)
                    {
                        model.AreFriends = "yes";
                        break;
                    }
                    if (r.Status == 2)
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
            model.UserProfile = user;
            model.CurrentUserStyle = context.Style.Where(s => s.UserId == 1).SingleOrDefault();
            model.UserStyle = style;
            model.Posts = posts;

            return View(model);
        }

        public IActionResult AddLike([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            int userId = post.UserId;
            post.Likes++;

            context.SaveChanges();
            return RedirectToAction("Profile", new { id = userId });
        }

        public IActionResult AddDislike([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            int userId = post.UserId;
            post.Dislikes++;

            context.SaveChanges();
            return RedirectToAction("Profile", new { id = userId });
        }
    }
}
