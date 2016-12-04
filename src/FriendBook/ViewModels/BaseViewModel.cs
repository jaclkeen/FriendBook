using FriendBook.Data;
using FriendBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.ViewModels
{
    public class BaseViewModel
    {
        public List<Relationship> FriendRequests { get; set; }

        public User CurrentUser { get; set; }

        public Style CurrentUserStyle { get; set; }

        private ActiveUser singleton = ActiveUser.Instance;

        public User ChosenUser
        {
            get
            {
                // Get the current value of the customer property of our singleton
                User user = singleton.User;

                // If no customer has been chosen yet, it's value will be null
                //if (user == null)
                //{
                //    // Return fake customer for now
                //    return new User()
                //    {
                //        FirstName = "Create",
                //        LastName = "Account"
                //    };
                //}

                // If there is a customer chosen, return it
                return user;
            }
            set
            {
                if (value != null)
                {
                    singleton.User = value;
                }
            }
        }

        public BaseViewModel() {}

        public BaseViewModel(FriendBookContext context) {

            int UserId = ActiveUser.Instance.User.UserId;

            var FRsSentToUser = context.Relationship.Where(r => r.ReciverUserId == UserId && r.Status == 0).ToList();

            foreach (Relationship r in FRsSentToUser)
            {
                r.SenderUser = context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault();
                r.ReceivingUser = context.User.Where(u => u.UserId == r.ReciverUserId).SingleOrDefault();
            }

            FriendRequests = FRsSentToUser;

            //REPLACE LATER WITH REAL USERID
            CurrentUserStyle = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();
            CurrentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();
        }
    }
}
