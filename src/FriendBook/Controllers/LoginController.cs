using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Models;
using FriendBook.ViewModels;
using FriendBook.Data;

namespace FriendBook.Controllers
{
    public class LoginController : Controller
    {
        private FriendBookContext context;

        public LoginController(FriendBookContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            model.UserStyle = new Style
            {
                BackgroundColor = "#0080c0",
                WallBackgroundColor = "#004080",
                PostBackgroundColor = "#808080",
                FontColor = "#000000",
                FontSize = 14,
                FontFamily = "Times New Roman",
                NavColor = "#808080",
                DetailColor = "#c0c0c0",
            };

            model.CurrentUserStyle = new Style()
            {
                NavColor = "#808080"
            };


            return View(model);
        }

        public IActionResult LoginUser([FromRoute]int id)
        {
            User user = context.User.Where(u => u.UserId == id).SingleOrDefault();
            ActiveUser.Instance.User = user;

            return RedirectToAction("Index", "Home");
        }
    }
}
