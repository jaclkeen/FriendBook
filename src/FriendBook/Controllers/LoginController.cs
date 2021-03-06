﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Models;
using FriendBook.ViewModels;
using FriendBook.Data;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace FriendBook.Controllers
{
    public class LoginController : Controller
    {
        private FriendBookContext context;

        public LoginController(FriendBookContext ctx)
        {
            context = ctx;
        }

        /**
        * Purpose: Method that contains the view for the Login page and sets the UserStyle for the page since the user is not logged in.
        * Arguments:
        *      None
        * Return:
        *      returns the view for the login page
        */
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

        /**
        * Purpose: Method that is used to set the activeUser equal to the user that was logged in
        * Arguments:
        *      int id - the id of the user being logged in
        * Return:
        *      Redirects the user to the /Home (Index) view
        */
        public IActionResult LoginUser([FromRoute]int id)
        {
            User user = context.User.Where(u => u.UserId == id).SingleOrDefault();
            ActiveUser.Instance.User = user;

            return RedirectToAction("Index", "Home");
        }

        /**
        * Purpose: Method that is used to logout the current user
        * Arguments:
        *      None
        * Return:
        *      Redirects the user back to the login page
        */
        public IActionResult LogoutUser()
        {
            ActiveUser.Instance.User = null;

            return RedirectToAction("Index", "Login");
        }

        /**
        * Purpose: Method that is called to Register a new user
        * Arguments:
        *      User user - contains all the neccessary properties to create a new user
        * Return:
        *      The createdUser's UserId to instantly login
        */
        [HttpPost]
        public int RegisterNewUser([FromBody]User user)
        {
            context.User.Add(user);

            if (ModelState.IsValid)
            {
                context.SaveChanges();
            }

            User CreatedUser = context.User.Where(u => u.UserId == user.UserId).SingleOrDefault();

            Style NewUserStyle = new Style(CreatedUser.UserId);
            context.Style.Add(NewUserStyle);
            context.SaveChanges();

            return CreatedUser.UserId;
        }

        //[HttpPost]
        //public void RegisterDummyUsers([FromBody] User DummyUser)
        //{

        //    context.User.Add(DummyUser);
        //    context.SaveChanges();

        //    Relationship NewRelationship = new Relationship
        //    {
        //        ReciverUserId = 1,
        //        SenderUserId = DummyUser.UserId,
        //        Status = 0,
        //    };

        //    Style s = new Style(DummyUser.UserId);

        //    context.Style.Add(s);
        //    context.Relationship.Add(NewRelationship);
        //    context.SaveChanges();
        //}
    }
}
