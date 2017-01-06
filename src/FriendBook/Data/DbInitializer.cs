using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using FriendBook.Models;

namespace FriendBook.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FriendBookContext(serviceProvider.GetRequiredService<DbContextOptions<FriendBookContext>>()))
            {
                if (context.User.Any())
                {
                    return;
                }

                var users = new User[]
                {
                    new User
                    {
                        FirstName = "Jacob",
                        LastName = "Keen",
                        Email = "jaclkeen@gmail.com",
                        Password = "jkeen33",
                    },
                    new User
                    {
                        FirstName = "Mason",
                        LastName = "Keen",
                        Email = "mason@mason.com",
                        Password = "mason",
                    },
                    new User
                    {
                        FirstName = "Aaron",
                        LastName = "Keen",
                        Email = "aaron@aaron.com",
                        Password = "aaron",
                    },
                    new User
                    {
                        FirstName = "Josh",
                        LastName = "Dobbs",
                        Email = "josh@josh.com",
                        Password = "josh",
                    },
                    new User
                    {
                        FirstName = "BillyBob",
                        LastName = "Johnson",
                        Email = "billy@billy.com",
                        Password = "billy",
                    },
                    new User
                    {
                        FirstName = "Tommy",
                        LastName = "John",
                        Email = "tommy@tommy.com",
                        Password = "tommy",
                    }
                };

                foreach (User u in users)
                {
                    context.User.Add(u);
                }
                context.SaveChanges();

                var posts = new Post[]
                {
                    new Post
                    {
                        UserId = 1,
                        Text = "This is the first seeded post!",
                        TimePosted = DateTime.Parse("06/23/2016"),
                        PostType = "Status"
                    },
                    new Post
                    {
                        UserId = 2,
                        Text = "This is the second seeded post!! Go me!",
                        TimePosted = DateTime.Parse("09/26/2008"),
                        PostType = "Status"
                    },
                    new Post
                    {
                        UserId = 3,
                        Text = "This is the third seeded post!!! Finally!",
                        TimePosted = DateTime.Parse("01/06/2009"),
                        PostType = "Status"
                    }
                };

                foreach (Post p in posts)
                {
                    context.Post.Add(p);
                }
                context.SaveChanges();

                var comments = new Comment[]
                {
                    new Comment
                    {
                        UserId = 1,
                        PostId = 1,
                        YardSaleItemId = null,
                        Text = "This is the first comment on the first seeded post!",
                        TimePosted = DateTime.Parse("03/13/2016")
                    },
                    new Comment
                    {
                        UserId = 2,
                        PostId = 2,
                        YardSaleItemId = null,
                        Text = "This is the first comment on the second seeded post!",
                        TimePosted = DateTime.Parse("05/29/2010")
                    },
                    new Comment
                    {
                        UserId = 3,
                        PostId = 3,
                        YardSaleItemId = null,
                        Text = "This is the third comment on the third seeded post!",
                        TimePosted = DateTime.Parse("07/18, 2016")
                    }
                };

                foreach (Comment c in comments)
                {
                    context.Comment.Add(c);
                }
                context.SaveChanges();

                var styles = new Style[]
                {
                    new Style(1), new Style(2), new Style(3), new Style(4), new Style(5), new Style(6)
                };

                foreach (Style s in styles)
                {
                    context.Style.Add(s);
                }

                var relationships = new Relationship[]
                {
                    new Relationship
                    {
                        SenderUserId = 1,
                        SenderUser = context.User.Where(u => u.UserId == 1).SingleOrDefault(),
                        ReciverUserId = 2,
                        ReceivingUser = context.User.Where(u => u.UserId == 2).SingleOrDefault(),
                    },
                    new Relationship
                    {
                        SenderUserId = 1,
                        SenderUser = context.User.Where(u => u.UserId == 1).SingleOrDefault(),
                        ReciverUserId = 3,
                        ReceivingUser = context.User.Where(u => u.UserId == 3).SingleOrDefault(),
                    },
                    new Relationship
                    {
                        SenderUserId = 2,
                        SenderUser = context.User.Where(u => u.UserId == 2).SingleOrDefault(),
                        ReciverUserId = 3,
                        ReceivingUser = context.User.Where(u => u.UserId == 3).SingleOrDefault(),
                    },
                    new Relationship
                    {
                        SenderUserId = 4,
                        SenderUser = context.User.Where(u => u.UserId == 4).SingleOrDefault(),
                        ReciverUserId = 1,
                        ReceivingUser = context.User.Where(u => u.UserId == 1).SingleOrDefault(),
                    }
                };

                foreach(Relationship r in relationships)
                {
                    context.Relationship.Add(r);
                }

                context.SaveChanges();
            }
        }
    }
}