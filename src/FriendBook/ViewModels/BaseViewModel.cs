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

        public List<MessageNotification> MessageNotifications { get; set; }

        public List<Notification> UserNotifications { get; set; }

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

        /**
        * Purpose:  Provide a BaseViewModel for all views to prevent copying code, by setting properties
        *           common to all views on creation of each new view model. All view models inherit from
        *           the BaseViewModel.
        * Arguments:
        *      FriendBookContext ctx - Gives access and connection to the database
        * Return:
        *      None
        */
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

            //GET ALL MESSAGE NOTIFICATIONS FOR CURRENT USER
            List<MessageNotification> MN = context.MessageNotification.Where(mn => mn.RecievingUserId == UserId && mn.Seen == false).ToList();
            MN.ForEach(n => n.RecievingUser = context.User.Where(u => u.UserId == n.RecievingUserId).SingleOrDefault());

            MessageNotifications = MN;

            //GET LIST OF ALL FRIENDS OF A CURRENT USER
            UserFriends = CurrentUserFriends.OrderBy(f => f.FirstName + f.LastName).ToList();

            //GETS LIST OF ALL THE CURRENT USER'S NOTIFICATIONS
            List<Notification> UN = context.Notification.Where(n => n.RecievingUserId == UserId && n.Seen == false).ToList();
            UN.ForEach(un => un.SendingUser = context.User.Where(u => u.UserId == un.SenderUserId).SingleOrDefault());
            UN.ForEach(un => un.RecievingUser = context.User.Where(u => u.UserId == un.RecievingUserId).SingleOrDefault());

            UserNotifications = UN.OrderByDescending(un => un.NotificatonDate).ToList();
        }
    }
}
