﻿using System;
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
                ConvoExists1.IsActive = true;

                return ConvoExists1;
            }

            if(ConvoExists2 != null)
            {
                ConvoExists2.ConversationStarter = context.User.Where(u => u.UserId == RecievingUserId).SingleOrDefault();
                ConvoExists2.ConversationReciever = ActiveUser.Instance.User;
                ConvoExists2.IsActive = true;

                return ConvoExists2;
            }

            Conversation c = new Conversation
            {
                ConversationRoomName = CurrentUserId.ToString() + RecievingUserId.ToString(),
                ConversationStarterId = CurrentUserId,
                ConversationRecieverId = RecievingUserId,
                IsActive = true
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