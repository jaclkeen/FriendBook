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

        public List<User> UserFriends { get; set; }

        public User CurrentUser { get; set; }

        public Style CurrentUserStyle { get; set; }

        private ActiveUser singleton = ActiveUser.Instance;

        public User ChosenUser
        {
            get
            {
                User user = singleton.User;
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

            //GETS ALL OF THE CURRENT USERS PENDING FRIEND REQUEST SENT TO HIM/HER
            var FRsSentToUser = context.Relationship.Where(r => r.ReciverUserId == UserId && r.Status == 0).ToList();
            foreach (Relationship r in FRsSentToUser)
            {
                r.SenderUser = context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault();
                r.ReceivingUser = context.User.Where(u => u.UserId == r.ReciverUserId).SingleOrDefault();
            }
            FriendRequests = FRsSentToUser;

            //SETS THE STYLE OF THE CURRENT LOGGED IN USER
            CurrentUserStyle = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();

            //SETS THE CURRENTUSER PROPERTY EQUAL TO THE CURRENT LOGGED IN USER
            CurrentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();

            //GETS ALL OF THE CURRENT USERS FRIENDS AND SETS THAT LIST EQUAL TO THE USERFRIENDS 
            //PROPERTY IN THE BASEVIEWMODEL
            List<Relationship> UserRelationships = context.Relationship.Where(u => u.ReciverUserId == UserId || u.SenderUserId == UserId).ToList();
            List<User> CurrentUserFriends = new List<User> { };
            foreach(Relationship r in UserRelationships)
            {
                if(r.ReciverUserId == UserId && r.Status == 1)
                {
                    CurrentUserFriends.Add(context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault());
                }
                else if(r.SenderUserId == UserId && r.Status == 1)
                {
                    CurrentUserFriends.Add(context.User.Where(u => u.UserId == r.ReciverUserId).SingleOrDefault());
                }
            }
            UserFriends = CurrentUserFriends.OrderBy(f => f.FirstName + f.LastName).ToList();
        }
    }
}
