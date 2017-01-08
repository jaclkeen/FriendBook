using FriendBook.Data;
using FriendBook.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.ViewModels
{
    public class YardSaleNewItemViewModel : BaseViewModel
    {
        public YardSaleNewItemViewModel(FriendBookContext ctx) : base(ctx) { }

        public YardSaleNewItemViewModel() { }

        public Style UserStyle { get; set; }

        public YardSaleItem NewItem { get; set; }

        public List<IFormFile> NewItemImages { get; set; }
    }
}