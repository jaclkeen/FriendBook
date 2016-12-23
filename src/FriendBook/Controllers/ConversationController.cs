using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FriendBook.Data;
using FriendBook.Models;

namespace FriendBook.Controllers
{
    public class ConversationController : Controller
    {
        private FriendBookContext context; 

        public ConversationController(FriendBookContext ctx)
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
                ConversationRecieverId = RecievingUserId,
                IsActive = false
            };

            context.Conversation.Add(c);
            context.SaveChanges();

            Conversation NewConvo = context.Conversation.Where(co => co.ConversationStarterId == CurrentUserId && co.ConversationRecieverId == RecievingUserId).SingleOrDefault();
            NewConvo.ConversationStarter = ActiveUser.Instance.User;
            NewConvo.ConversationReciever = context.User.Where(u => u.UserId == RecievingUserId).SingleOrDefault();

            return NewConvo;
        }

        public List<Conversation> ActiveConversations()
        {
            int UId = ActiveUser.Instance.User.UserId;
            List<Conversation> conversations = context.Conversation.Where(c => c.ConversationRecieverId == UId || c.ConversationStarterId == UId).ToList();
            conversations.ForEach(co => co.ConversationReciever = context.User.Where(u => u.UserId == co.ConversationRecieverId).SingleOrDefault());
            conversations.ForEach(co => co.ConversationStarter = context.User.Where(u => u.UserId == co.ConversationStarterId).SingleOrDefault());

            return conversations.Where(convo => convo.IsActive == true).ToList();
        }

        public List<Message> GetAllConversationMessages([FromBody] string ConversationName)
        {
            List<Message> messages = context.Message.Where(m => m.ConversationRoomName == ConversationName).ToList();
            messages.ForEach(m => m.SendingUser = context.User.Where(u => u.UserId == m.SendingUserId).SingleOrDefault());

            return messages;
        }

        [HttpPost]
        public void EndConversation([FromBody] int id)
        {
            Conversation c = context.Conversation.Where(co => co.ConversationRoomName == id.ToString()).SingleOrDefault();
            c.IsActive = false;

            context.SaveChanges();
        }

        [HttpGet]
        public List<MessageNotification> MessageNotifications()
        {
            List<MessageNotification> MN = context.MessageNotification.Where(mn => mn.RecievingUserId == ActiveUser.Instance.User.UserId && mn.Seen == false).ToList();
            MN.ForEach(m => m.RecievingUser = context.User.Where(u => u.UserId == m.SendingUserId).SingleOrDefault());

            return MN;
        }

        [HttpPost]
        public void ActivateConversation([FromBody] int RoomName)
        {
            Conversation convo = context.Conversation.Where(c => c.ConversationRoomName == RoomName.ToString()).SingleOrDefault();
            convo.IsActive = true;
            context.SaveChanges();
        }

        [HttpPost]
        public int MessageSeen([FromBody] int id)
        {
            MessageNotification mn = context.MessageNotification.Where(m => m.MessageNotificationId == id).SingleOrDefault();
            mn.Seen = true;

            context.SaveChanges();

            return context.MessageNotification.Where(MN => MN.RecievingUserId == ActiveUser.Instance.User.UserId && MN.Seen==false).ToList().Count();
        }

        [HttpPost]
        public IActionResult PostToConversation([FromBody] Message message)
        {
            Conversation convo = context.Conversation.Where(c => c.ConversationRoomName == message.ConversationRoomName).SingleOrDefault();
            User RecievingUser = context.User.Where(u => u.UserId == convo.ConversationStarterId).SingleOrDefault();

            if(convo.ConversationStarterId == ActiveUser.Instance.User.UserId)
            {
                RecievingUser = context.User.Where(u => u.UserId == convo.ConversationRecieverId).SingleOrDefault();
            }
            
            Message NewMessage = new Message()
            {
                MessageText = message.MessageText,
                SendingUserId = ActiveUser.Instance.User.UserId,
                MessageSentDate = DateTime.Now,
                ConversationRoomName = message.ConversationRoomName,
                Conversation = context.Conversation.Where(c => c.ConversationRoomName == message.ConversationRoomName).SingleOrDefault()
            };

            MessageNotification mn = new MessageNotification()
            {
                SendingUserId = ActiveUser.Instance.User.UserId,
                RecievingUserId = RecievingUser.UserId
            };

            context.Add(mn);
            context.Add(NewMessage);

            context.SaveChanges();

            return new NoContentResult();
        }
    }
}
