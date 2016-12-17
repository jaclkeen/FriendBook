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
        private IHostingEnvironment _environment;

        public HomeController(FriendBookContext ctx, IHostingEnvironment environment)
        {
            _environment = environment;
            context = ctx;
        }

        public IActionResult Index()
        {
            int UserId = ActiveUser.Instance.User.UserId;

            var relationships = context.Relationship.Where(r => r.ReciverUserId == UserId || r.SenderUserId == UserId).ToList();
            var styling = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();

            var users = context.User.ToList();
            var currentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();

            HomePageViewModel model = new HomePageViewModel(context);
            model.Posts = new List<Post> { };

            //List<Post> UserPost = new List<Post>();
            List<Post> UserPost = (from r in context.Relationship
                                   join u in context.User on r.ReciverUserId equals u.UserId
                                   join p in context.Post on u.UserId equals p.UserId
                                   where r.Status == 1
                                   select p).ToList();

            //foreach (Relationship r in relationships)
            //{
            //    if(r.ReciverUserId == UserId && r.Status == 1)
            //    {
            //        UserPost = context.Post.Where(p => p.UserId == r.SenderUserId).ToList();
            //        if (UserPost != null)
            //        {
            //            UserPost.ForEach(up => up.User = context.User.Where(user => user.UserId == up.UserId).SingleOrDefault());
            //            UserPost.ForEach(up => model.Posts.Add(up));
            //        }
            //    }
            //    else if (r.SenderUserId == UserId && r.Status == 1)
            //    {
            //        UserPost = context.Post.Where(p => p.UserId == r.ReciverUserId).ToList();

            //        if (UserPost != null)
            //        {
            //            UserPost.ForEach(up => up.User = context.User.Where(user => user.UserId == up.UserId).SingleOrDefault());
            //            UserPost.ForEach(up => model.Posts.Add(up));
            //        }
            //    }
            //}

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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public User GetCurrentUser()
        {
            int UserId = ActiveUser.Instance.User.UserId;

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
