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

        public Post WallPost { get; set; }

        /**
        * Purpose:  Provide a BaseViewModel for all Profile type views to prevent copying code, by setting properties
        *           common to all profile based views on creation of each profile based view model. Each of these profile
        *           based view models inherit from the ProfileBaseViewModel.
        * Arguments:
        *      FriendBookContext ctx - Gives access and connection to the database
        *      int UserProfileId - the UserId of the current profile that is being visited.
        * Return:
        *      None
        */
        public ProfileBaseViewModel(FriendBookContext ctx, int UserProfileId) : base(ctx)
        {
            //WHEN A NEW INSTANCE OF PROFILEBASEVIEWMODEL IS CREATED, IT GETS THE CURRENT USER PROFILES ID AND SETS THE
            //USER PROFILE EQUAL TO THAT USER, GETS THE STYLE OF THAT PARTICULAR USER, QUERIES THROUGH ALL RELATIONSHIPS
            //TO FIND THE TYPE OF RELATIONSHIP SHARED BETWEEN THE CURRENT LOGGED IN USER, AND THE PROFILE BEING VIEWED

            User LoggedInUser = ActiveUser.Instance.User;

            //THE USER WHOSE PROFILE IS BEING VISITED
            User user = ctx.User.Where(u => u.UserId == UserProfileId).SingleOrDefault();
            UserProfile = user;

            //SETS THE CURRENT USER'S STYLE OF THE PROFILE BEING VISITED
            Style style = ctx.Style.Where(s => s.UserId == UserProfileId).SingleOrDefault();
            UserStyle = style;

            //DETERMINES THE RELATIONSHIP BETWEEN THE CURRENT USER AND THE USER'S PROFILE THAT IS BEING VISITED
            List<Relationship> relationships = ctx.Relationship.Where(r => r.ReciverUserId == UserProfileId || r.SenderUserId == UserProfileId).ToList();
            this.AreFriends = "NoRelationship";

            foreach (Relationship r in relationships)
                {
                    if (r.SenderUserId == LoggedInUser.UserId || r.ReciverUserId == LoggedInUser.UserId)
                    {
                        if (r.Status == 0)
                        {
                            this.AreFriends = "Pending";
                        }
                        else if (r.Status == 1)
                        {
                            this.AreFriends = "yes";
                        }
                        else if (r.Status == 2)
                        {
                            this.AreFriends = "no";
                        }
                        else if(r.Status == 3)
                        {
                            this.AreFriends = "blocked";
                        }
                    }
                }
            }

    }
}
