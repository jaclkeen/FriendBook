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
                        TimePosted = DateTime.Parse("06/23/2016")
                    },
                    new Post
                    {
                        UserId = 2,
                        Text = "This is the second seeded post!! Go me!",
                        TimePosted = DateTime.Parse("09/26/2008")
                    },
                    new Post
                    {
                        UserId = 3,
                        Text = "This is the third seeded post!!! Finally!",
                        TimePosted = DateTime.Parse("01/06/2009")
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
                        Text = "This is the first comment on the first seeded post!",
                        TimePosted = DateTime.Parse("03/13/2016")
                    },
                    new Comment
                    {
                        UserId = 2,
                        PostId = 2,
                        Text = "This is the first comment on the second seeded post!",
                        TimePosted = DateTime.Parse("05/29/2010")
                    },
                    new Comment
                    {
                        UserId = 3,
                        PostId = 3,
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
                    new Style
                    {
                        UserId = 1,
                        BackgroundColor = "Firebrick",
                        FontColor = "white",
                        FontSize = 16,
                        FontFamily = "Times New Roman",
                        NavColor = "lightgrey",
                        DetailColor = "black"
                    },
                    new Style
                    {
                        UserId = 2,
                        BackgroundColor = "black",
                        FontColor = "red",
                        FontSize = 12,
                        FontFamily = "Times New Roman",
                        NavColor = "firebrick",
                        DetailColor = "black"
                    },
                    new Style
                    {
                        UserId = 3,
                        BackgroundColor = "blue",
                        FontColor = "red",
                        FontSize = 20,
                        NavColor = "black",
                        DetailColor = "green"
                    }
                };

                foreach (Style s in styles)
                {
                    context.Style.Add(s);
                }

                var relationships = new Relationship[]
                {
                    new Relationship
                    {
                        UserId1 = 1,
                        UserId2 = 2
                    },
                    new Relationship
                    {
                        UserId1 = 1,
                        UserId2 = 3
                    },
                    new Relationship
                    {
                        UserId1 = 2,
                        UserId2 = 3
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