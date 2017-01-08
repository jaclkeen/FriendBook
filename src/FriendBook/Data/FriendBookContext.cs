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
        public DbSet<Album> Album { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<MessageNotification> MessageNotification { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<YardSaleItem> YardSaleItem { get; set; }
    }
}