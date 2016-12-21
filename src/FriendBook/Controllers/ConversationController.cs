using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Data;
using FriendBook.Models;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using FriendBook.Hubs;
using FriendBook.ViewModels;

namespace FriendBook.Controllers
{
    public class ConversationController : ApiHubController<Broadcaster>
    {
        private FriendBookContext context; 

        public ConversationController(FriendBookContext ctx, IConnectionManager connectionManager) : base(connectionManager)
        {
            context = ctx;
        }

        public Conversation CreateConversation([FromBody] int RecievingUserId)
        {
            int CurrentUserId = ActiveUser.Instance.User.UserId;
            Conversation ConvoExists1 = context.Conversation.Where(con => con.ConversationStarterId == CurrentUserId && con.ConversationRecieverId == RecievingUserId).SingleOrDefault();
            Conversation ConvoExists2 = context.Conversation.Where(conv => conv.ConversationStarterId == RecievingUserId && conv.ConversationRecieverId == CurrentUserId).SingleOrDefault();

            if (ConvoExists1 != null)
            {
                ConvoExists1.ConversationStarter = ActiveUser.Instance.User;
                ConvoExists1.ConversationReciever = context.User.Where(u => u.UserId == RecievingUserId).SingleOrDefault();

                return ConvoExists1;
            }

            if(ConvoExists2 != null)
            {
                ConvoExists2.ConversationStarter = context.User.Where(u => u.UserId == RecievingUserId).SingleOrDefault();
                ConvoExists2.ConversationReciever = ActiveUser.Instance.User;

                return ConvoExists2;
            }

            Conversation c = new Conversation
            {
                ConversationRoomName = CurrentUserId.ToString() + RecievingUserId.ToString(),
                ConversationStarterId = CurrentUserId,
                ConversationRecieverId = RecievingUserId
            };

            context.Conversation.Add(c);
            context.SaveChanges();

            Conversation NewConvo = context.Conversation.Where(co => co.ConversationStarterId == CurrentUserId && co.ConversationRecieverId == RecievingUserId).SingleOrDefault();
            NewConvo.ConversationStarter = ActiveUser.Instance.User;
            NewConvo.ConversationReciever = context.User.Where(u => u.UserId == RecievingUserId).SingleOrDefault();

            return NewConvo;
        }

        public List<Message> GetAllConversationMessages([FromBody] string ConversationName)
        {
            return context.Message.Where(m => m.ConversationRoomName == ConversationName).ToList();
        }

        public IActionResult PostToConversation([FromBody] NewMessageViewModel model)
        {
            string ChatroomName = model.ConversationRoomName;

            Message message = new Message()
            {
                MessageText = model.message.MessageText,
                SendingUserId = ActiveUser.Instance.User.UserId,
                SendingUser = ActiveUser.Instance.User,
                MessageSentDate = DateTime.Now,
                ConversationRoomName = ChatroomName,
                Conversation = context.Conversation.Where(c => c.ConversationRoomName == ChatroomName).SingleOrDefault()
            };

            // Save the new message
            context.Message.Add(message);
            context.SaveChanges();

            //MessageViewModel model = new MessageViewModel(newMessage);

            this.Clients.Group(ChatroomName).AddChatMessage(model);
            return new NoContentResult();
        }

    }
}
