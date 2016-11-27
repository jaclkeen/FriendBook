using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Data;
using FriendBook.Models;
using FriendBook.ViewModels;

namespace FriendBook.Controllers
{
    public class PostController : Controller
    {
        private FriendBookContext context;

        public PostController(FriendBookContext ctx)
        {
            context = ctx;
        }

        public IActionResult AddLike([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            post.Likes++;

            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddDislike([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            post.Dislikes++;

            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeletePost([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            context.Post.Remove(post);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddProfileLike([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            int userId = post.UserId;
            post.Likes++;

            context.SaveChanges();
            return RedirectToAction("Profile", "Profile", new { id = userId });
        }

        public IActionResult AddProfileDislike([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            int userId = post.UserId;
            post.Dislikes++;

            context.SaveChanges();
            return RedirectToAction("Profile", "Profile", new { id = userId });
        }

        [HttpGet]
        public Post GetSpecificPost([FromRoute] int id)
        {
            Post post = context.Post.Where(p => p.PostId == id).SingleOrDefault();
            return (post);
        }
    }
}
