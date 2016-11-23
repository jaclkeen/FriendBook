using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace FriendBook.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FriendBookContext(serviceProvider.GetRequiredService<DbContextOptions<FriendBookContext>>()))
            {
                //if (context.Employee.Any())
                //{
                //    return;
                //}
            }
        }
    }
}