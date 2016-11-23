using Microsoft.EntityFrameworkCore;
using FriendBook.Models;

namespace FriendBook.Data
{
    public class FriendBookContext : DbContext
    {
        public FriendBookContext(DbContextOptions<FriendBookContext> options)
                : base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Style> Style { get; set; }
        public DbSet<Relationship> Relationship { get; set; }
    }
}