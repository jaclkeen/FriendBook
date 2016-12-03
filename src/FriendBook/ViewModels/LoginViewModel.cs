using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Models;

namespace FriendBook.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Style UserStyle { get; set; }
        public User User { get; set; }
    }
}
