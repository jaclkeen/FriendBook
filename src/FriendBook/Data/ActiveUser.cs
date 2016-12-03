using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Models;

namespace FriendBook.Data
{
    public class ActiveUser
    {
        // Make the class a singleton to maintain state across all uses
        private static ActiveUser _instance;
        public static ActiveUser Instance
        {
            get
            {
                // First time an instance of this class is requested
                if (_instance == null)
                {
                    _instance = new ActiveUser();
                }
                return _instance;
            }
        }

        // To track the currently active customer - selected by user
        public User User { get; set; }
    }
}
