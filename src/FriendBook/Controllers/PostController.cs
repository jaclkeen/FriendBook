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

        [HttpPost]
        public void CreateNewCommentOnPost([FromBody] Comment comment)
        {
            //LATER REPLACE WITH ACTIVE USER
            comment.UserId = 1;
            comment.User = context.User.Where(u => u.UserId == 1).SingleOrDefault();
            comment.TimePosted = DateTime.Now;

            context.Comment.Add(comment);

            try
            {
                context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public List<Comment> GetComments([FromRoute] int id)
        {
            List<Comment> comments = context.Comment.Where(p => p.PostId == id).ToList();
            comments.ForEach(c => c.User = context.User.Where(u => u.UserId == c.UserId).SingleOrDefault());

            return comments;
        }

        [HttpPost]
        public void EditSpecificPost([FromBody] Post EditedPost)
        {
            Post post = context.Post.Where(p => p.PostId == EditedPost.PostId).SingleOrDefault();
            post.Text = EditedPost.Text;

            context.Post.Update(post);
            context.SaveChanges();
        }
    }
}
