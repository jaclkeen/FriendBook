using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Hubs
{
    public class Broadcaster : Hub
    {
        public override Task OnConnected()
        {
            // Set connection id for just connected client only
            return Clients.Client(Context.ConnectionId).SetConnectionId(Context.ConnectionId);
        }

        // Server side methods called from client
        public Task Subscribe(string chatroom)
        {
            return Groups.Add(Context.ConnectionId, chatroom.ToString());
        }

        public Task Unsubscribe(string chatroom)
        {
            return Groups.Remove(Context.ConnectionId, chatroom.ToString());
        }
    }
}
