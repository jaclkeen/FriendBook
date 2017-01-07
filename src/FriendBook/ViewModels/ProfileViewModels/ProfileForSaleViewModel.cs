using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Data;
using FriendBook.Models;

namespace FriendBook.ViewModels
{
    public class ProfileForSaleViewModel : ProfileBaseViewModel
    {
        public ProfileForSaleViewModel(FriendBookContext ctx, int UserProfileId) : base(ctx, UserProfileId) { }

        public List<YardSaleItem> YardSaleItems { get; set; }
    }
}
