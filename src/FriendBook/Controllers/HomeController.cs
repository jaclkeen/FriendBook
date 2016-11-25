using System;
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
            //REPLACE WITH REAL CURRENT USER WHEN LOGIN IS CREATED
            var styling = context.Style.Where(s => s.UserId == 1).SingleOrDefault();
            var posts = context.Post.OrderByDescending(p => p.TimePosted).ToList();
            var users = context.User.ToList();
            //REPLACE WITH REAL CURRENT USER WHEN LOGIN IS CREATED
            var currentUser = context.User.Where(u => u.UserId == 1).SingleOrDefault();

            foreach(Post p in posts)
            {
                foreach(User u in users)
                {
                    if(p.UserId == u.UserId)
                    {
                        p.User = u;
                    }
                }
            }

            HomePageViewModel model = new HomePageViewModel();
            model.Posts = posts;
            model.UserStyle = styling;
            model.CurrentUserStyle = styling;
            model.CurrentUser = currentUser;

            return View(model);
        }

        public IActionResult NewStatus(Post post)
        {
            //REPLACE WITH REAL CURRENT USER WHEN LOGIN IS CREATED
            post.UserId = 1;
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

        public IActionResult AddLike([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            post.Likes++;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddDislike([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            post.Dislikes++;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeletePost([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            context.Post.Remove(post);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public List<User> GetUsers()
        {
            var users = context.User.ToList();
            return users;
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
