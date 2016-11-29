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

        public BaseViewModel() {}

        public BaseViewModel(FriendBookContext context) {

            var FRsSentToUser = context.Relationship.Where(r => r.ReciverUserId == 1 && r.Status == 0).ToList();

            foreach (Relationship r in FRsSentToUser)
            {
                r.SenderUser = context.User.Where(u => u.UserId == r.SenderUserId).SingleOrDefault();
                r.ReceivingUser = context.User.Where(u => u.UserId == r.ReciverUserId).SingleOrDefault();
            }

            FriendRequests = FRsSentToUser;

            //REPLACE LATER WITH REAL USERID
            CurrentUserStyle = context.Style.Where(s => s.UserId == 1).SingleOrDefault();
            CurrentUser = context.User.Where(u => u.UserId == 1).SingleOrDefault();
        }
    }
}
