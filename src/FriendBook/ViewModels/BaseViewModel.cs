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

            var FRsSentToUser = context.Relationship.Where(r => r.ReciverUserId == UserId && r.Status == 0).ToList();

            foreach (Relationship r in FRsSentToUser)
            {
                r.SenderUser = context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault();
                r.ReceivingUser = context.User.Where(u => u.UserId == r.ReciverUserId).SingleOrDefault();
            }

            FriendRequests = FRsSentToUser;

            CurrentUserStyle = context.Style.Where(s => s.UserId == UserId).SingleOrDefault();
            CurrentUser = context.User.Where(u => u.UserId == UserId).SingleOrDefault();
        }
    }
}
