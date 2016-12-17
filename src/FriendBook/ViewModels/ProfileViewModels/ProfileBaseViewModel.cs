using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendBook.Data;
using FriendBook.Models;

namespace FriendBook.ViewModels
{
    public class ProfileBaseViewModel : BaseViewModel
    {
        public ProfileBaseViewModel() { }

        public User UserProfile { get; set; }

        public Style UserStyle { get; set; }

        public string AreFriends { get; set; }

        public ProfileBaseViewModel(FriendBookContext ctx, int UserProfileId) : base(ctx)
        {
            //WHEN A NEW INSTANCE OF PROFILEBASEVIEWMODEL IS CREATED, IT GETS THE CURRENT USER PROFILES ID AND SETS THE
            //USER PROFILE EQUAL TO THAT USER, GETS THE STYLE OF THAT PARTICULAR USER, QUERIES THROUGH ALL RELATIONSHIPS
            //TO FIND THE TYPE OF RELATIONSHIP SHARED BETWEEN THE CURRENT LOGGED IN USER, AND THE PROFILE BEING VIEWED

            User LoggedInUser = ActiveUser.Instance.User;

            //THE USER WHOSE PROFILE IS BEING VISITED
            User user = ctx.User.Where(u => u.UserId == UserProfileId).SingleOrDefault();
            UserProfile = user;

            Style style = ctx.Style.Where(s => s.UserId == UserProfileId).SingleOrDefault();
            UserStyle = style;

            List<Relationship> relationships = ctx.Relationship.Where(r => r.ReciverUserId == UserProfileId || r.SenderUserId == UserProfileId).ToList();

            if (relationships.Count > 0)
            {
                foreach (Relationship r in relationships)
                {
                    if (r.SenderUserId == LoggedInUser.UserId || r.ReciverUserId == LoggedInUser.UserId)
                    {
                        if (r.Status == 0)
                        {
                            AreFriends = "Pending";
                            break;
                        }
                        else if (r.Status == 1)
                        {
                            AreFriends = "yes";
                            break;
                        }
                        else if (r.Status == 2)
                        {
                            AreFriends = "no";
                            break;
                        }
                        else
                        {
                            AreFriends = "blocked";
                        }
                    }
                }

                if (AreFriends == null)
                {
                    AreFriends = "NoRelationship";
                }
            }
        }
    }
}
