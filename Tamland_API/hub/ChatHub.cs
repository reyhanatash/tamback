using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Tamland_API.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Tamland_API.hub
{
    [Authorize]
    public class ChatHub : Hub
    {
        static List<UserHubModel> UsersList = new List<UserHubModel>();
        static List<SendMessage> MessageList = new List<SendMessage>();
        [Authorize]
        public void Join(string role,string ip)
        {
            var id = Context.ConnectionId;
            
            //get user id
            var claims = (Context.User.Identity as System.Security.Claims.ClaimsIdentity).Claims.FirstOrDefault();
            //if user is admin
            if (role == "1")
            {
                //now add USER to UsersList
                UsersList.Add(new UserHubModel
                {
                    ConnectionId = id,
                    UserID = int.Parse(claims.Value.ToString()),
                    UserGroup = null,
                    freeflag = "1",
                    tpflag = "1",
                    Ip= ip
                });
                //whether it is Admin or User now both of them has userGroup and I Join this user or admin to specific group 
                //Groups.Add(Context.ConnectionId, group);
                // Clients.Caller.onConnected(id);
                var messages = MessageList.Where(x => x.Deleted == false).ToList();
                Clients.Client(Context.ConnectionId).sendAllMessages(JsonConvert.SerializeObject(messages));
            }
            //if user not admin
            else
            {
                //now add USER to UsersList
                UsersList.Add(new UserHubModel
                {
                    ConnectionId = id,
                    UserID = int.Parse(claims.Value.ToString()),
                    UserGroup = null,
                    freeflag = "1",
                    tpflag = "0",
                    Ip = ip,
                    UserName=Context.User.Identity.Name
                });
                //whether it is Admin or User now both of them has userGroup and I Join this user or admin to specific group 
                //Groups.Add(Context.ConnectionId, group);
                // Clients.Caller.onConnected(id);
                //filter approved messages
                var messages = MessageList.Where(x => (x.Approved == true && x.Deleted == false) || (x.ConnectionId == Context.ConnectionId && x.Deleted == false)).ToList();
                Clients.Client(Context.ConnectionId).sendAllMessages(JsonConvert.SerializeObject(messages));
            }
        }
        [Authorize]
        public void SendMessageToAdmin(string msg,int id,string messageId)
        {
            var claims = (Context.User.Identity as System.Security.Claims.ClaimsIdentity).Claims.FirstOrDefault();
            //get user info
            var user = UsersList.Find(x => x.UserID == int.Parse(claims.Value));
            //find admins ID
            var admins = UsersList.Where(X => X.tpflag == "1").ToList();
            var message = new SendMessage();
            message.ip = user.Ip;
            message.Message = msg;
            message.UserId = id;
            message.UserName = user.UserName;
            message.MessageId = messageId;
            message.Approved = false;
            message.ConnectionId = Context.ConnectionId;
            message.FullName = user.FullName;
            //push message to list
            MessageList.Add(message);
            //send message to all admins
            foreach (var admin in admins)
            {
                string UserID = ((UserHubModel)admin).ConnectionId;
                Clients.Client(UserID).sendAdminMessage(JsonConvert.SerializeObject(message));
            }
        }
        [Authorize(Roles = "Admin")]
        public void BroadCastMessage(string msgFrom, string msg,string messageId)
        {
            var message = MessageList.Find(x => x.MessageId == messageId);
            message.Approved = true;
            Clients.Others.getMessages(msgFrom, msg, messageId);
        }
        [Authorize(Roles = "Admin")]
        public void RefreshPage(string id)
        {
            Clients.Client(id).refreshUserPage();
        }
        [Authorize(Roles = "Admin")]
        public void RefreshVideo(string id)
        {
            Clients.Client(id).refreshUserVideo();
        }
        //replay message to user
        [Authorize(Roles = "Admin")]
        public void SendMessageToUser(string msgId,string msg,string id)
        {
            var obj = MessageList.Find(x => x.MessageId == msgId);
            obj.Replay = msg;
            Clients.All.sendUserMessage(msgId, msg);
        }
        [Authorize(Roles = "Admin")]
        public void DeleteMessage(string messageId)
        {
            var msg = MessageList.Find(x => x.MessageId == messageId);
            msg.Deleted = true;
            Clients.All.sendDeleteMessage(messageId);
        }
        //get test from admin and send to others
        [Authorize(Roles = "Admin")]
        public void CreateTest(string testJson)
        {
            var obj = JsonConvert.DeserializeObject<Rootobject>(testJson);
            SendMessage message;
            if (MessageList.Exists(x => x.QuestionId == obj.id))
            {
                message = MessageList.Find(x => x.QuestionId == obj.id);
                message.Message = obj.text;
                message.QuestionId = obj.id;
                message.Approved = true;
                message.Options = JsonConvert.SerializeObject(obj.options);
                message.type = 2;
            }
            else
            {
                message = new SendMessage();
                message.Message = obj.text;
                message.QuestionId = obj.id;
                message.Options = JsonConvert.SerializeObject(obj.options);
                message.type = 2;
                message.Approved = true;
                MessageList.Add(message);
            }
            Clients.All.getTest(testJson);
        }
        //admin send message to users
        [Authorize(Roles = "Admin")]
        public void SendMessageTOUsers(string msg,string messageId)
        {
            var message = new SendMessage();
            message.Message = msg;
            message.type = 1;
            message.Approved = true;
            message.UserName = "ادمین";
            message.MessageId = messageId;
            message.IsAdmin = true;
            MessageList.Add(message);
            Clients.All.getMessageFromAdmin(msg, messageId);
        }
        //send temp test to other admins
        [Authorize(Roles = "Admin")]
        public void sendTempTest(string testJson)
        {
            //find all admins
            var admins = UsersList.Where(X => X.tpflag == "1").ToList();

            var obj = JsonConvert.DeserializeObject<Rootobject>(testJson);
            SendMessage message;
            if (MessageList.Exists(x => x.QuestionId == obj.id))
            {
                message = MessageList.Find(x => x.QuestionId == obj.id);
                message.Message = obj.text;
                message.QuestionId = obj.id;
                message.Options = JsonConvert.SerializeObject(obj.options);
                message.type = 3;
            }
            else
            {
                message = new SendMessage();
                message.Message = obj.text;
                message.QuestionId = obj.id;
                message.Options = JsonConvert.SerializeObject(obj.options);
                message.type = 3;
                message.Approved = false;
                MessageList.Add(message);
            }
            foreach (var item in admins)
            {
                Clients.Client(item.ConnectionId).getTempTest(testJson);
            }
           
        }
        //finish Test
        [Authorize(Roles = "Admin")]
        public void finishTest(string id,string options)
        {
            //find message
            var msg = MessageList.Find(x => x.QuestionId == id);
            msg.IsEnd = true;
            msg.Options = options;
            Clients.All.getFinishTest(msg.QuestionId, options);
        }

        //dispoce user list and message list when cource is end
        [Authorize(Roles = "Admin")]
        public void finishCource()
        {
            UsersList = null;
            MessageList = null;
        }
    }
}