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
                        Relationships = new List<Relationship>
                        {
                            new Relationship
                            {
                                UserId1 = 1,
                                UserId2 = 2,
                                Status = 1
                            }
                        }
                    },
                    new User
                    {
                        FirstName = "Aaron",
                        LastName = "Keen",
                        Email = "aaron@aaron.com",
                        Password = "aaron",
                        Relationships = new List<Relationship>
                        {
                            new Relationship
                            {
                                UserId1 = 2,
                                UserId2 = 3,
                                Status = 1
                            }
                        }
                    }
                };
                context.Add(users);

                var posts = new Post[]
                {
                    new Post
                    {
                        UserId = 1,
                        Text = "This is the first seeded post!",
                    },
                    new Post
                    {
                        UserId = 2,
                        Text = "This is the second seeded post!! Go me!"
                    },
                    new Post
                    {
                        UserId = 3,
                        Text = "This is the third seeded post!!! Finally!"
                    }
                };
                context.Add(posts);

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
                context.Add(comments);

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
                context.Add(styles);



            }
        }
    }
}