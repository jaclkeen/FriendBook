﻿using FriendBook.Data;
using FriendBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.ViewModels
{
    public class YardSaleHomeViewModel : BaseViewModel
    {
        public YardSaleHomeViewModel(FriendBookContext ctx) : base(ctx) { }

        public Style UserStyle { get; set; }

        public List<YardSaleItem> YardSaleItems { get; set;}
    }
}