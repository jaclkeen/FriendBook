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
            var posts = context.Post.ToList();
            var users = context.User.ToList();

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

            return View(model);
        }

        public IActionResult NewStatus(Post post)
        {
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
