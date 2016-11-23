using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Data
{
    public class FriendBookContext : DbContext
    {
        public FriendBookContext(DbContextOptions<FriendBookContext> options)
                : base(options)
            { }
    }
}
