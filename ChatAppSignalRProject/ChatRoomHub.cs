using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ChatAppSignalRProject.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatAppSignalRProject
{
    [HubName("chat")]
    public class ChatRoomHub : Hub
    {
        ChatRoomContext context = new ChatRoomContext();
        public string Register(string name, string password, string email, string image)
        {
            int nameCount = context.Users.Where(u => u.UserName == name).Count();
            int emailCount = context.Users.Where(u => u.Email == email).Count();
            if (nameCount > 0 && emailCount > 0)
            {
                User user = context.Users.Single(u => u.UserName == name && u.Email == email);
                user.Image = image;
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                User user = new User(name, password, email, image);
                context.Users.Add(user);
                context.SaveChanges();
            }
            return name;
        }

        public string Login(string loginName, string loginPassword)
        {
            string message = string.Empty;
            int activeUserCount = context.Users.Where(u => u.UserName == loginName && u.Password == loginPassword && u.Active == true).Count();
            int userCount = context.Users.Where(u => u.UserName == loginName && u.Password == loginPassword).Count();
            if(userCount == 0)
            {
                message = "User and/or Password do not match.";
                return message;
            }
            else
            {
                if(activeUserCount == 0)
                {
                    message = "User is inactive.";
                    return message;
                }
                else
                {
                    var userRoomes = context.ConversationRooms.Include(CreateDatabaseIfNotExists => CreateDatabaseIfNotExists.UserRole).Where(cr => cr.UserRole.UserName == loginName && cr.Allowed == true).Select(cr => cr.RoomName).ToArray();
                    Clients.Caller.rooms(userRoomes);
                    Clients.Caller.name(loginName);
                    return loginName;
                }
            }
        }

        public void Logout(string loginName)
        {
            var connections = context.Connections.Where(c => c.ContextConnectionId == Context.ConnectionId).ToList();
            foreach (Connection aConnection in connections)
            {
                aConnection.Connected = false;
                context.Entry(aConnection).State = EntityState.Modified;
                context.SaveChanges();
            }

            List<string> joinedRooms = context.ConversationRooms.Include(CreateDatabaseIfNotExists => CreateDatabaseIfNotExists.UserRole).Where(cr => cr.UserRole.UserName == loginName && cr.Allowed == true).Select(cr => cr.RoomName).ToList();

            foreach (string aRoom in joinedRooms)
            {
                Groups.Remove(loginName, aRoom);
            }
                    
            Clients.Caller.rooms(null);
        }

        public object ActivateUser(string name, bool active)
        {
            User user = context.Users.Single(u => u.UserName == name);
            user.Active = active;
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
            if (context.Users.Where(u => u.Active == false).Count() > 0)
            {
                var a = context.Users.Where(u => u.Active == false).Select(u => new { u.UserName, User = u.UserName + ": " + u.Email }).ToArray();

                return a;
            }
            else
            {
                return null;
            }
        }

        public object GetActiveUsers()
        {
            var activeUsers = context.Users.Where(u => u.Active == true).Except(context.UserRoles.Include(Uri => Uri.User).Select(ur => ur.User)).Select(u => new { u.UserName, User = u.UserName + ": " + u.Email }).ToArray();
            return activeUsers;
        }

        public object GetExistingRoles()
        {
            var roles = context.Roles.Select(r => new { r.RoleID, r.Name }).ToArray();
            return roles;
        }

        public object GetChatRooms()
        {
            var chatRooms = context.ChatRooms.Select(cr => new { cr.RoomName }).ToArray();
            return chatRooms;
        }

        public object GetAllowedRoomsForUser(string userName)
        {
            var allowedChatRoomsForUser = context.ConversationRooms.Include(cr => cr.UserRole).Where(cr => cr.UserRole.UserName == userName).Select(cr => cr.RoomName).ToArray();
            return allowedChatRoomsForUser;
        }

        public object GetUserWithRole()
        {
            var userWithRole = context.UserRoles.Select(ur => new { ur.UserRoleID, ur.UserName }).ToArray();
            return userWithRole;
        }

        public object AddRole(string roleName)
        {
            int roleCount = context.Roles.Where(r => r.Name == roleName).Count();

            if (roleCount > 0)
            {
                return "Role already exists.";
            }
            else
            {
                Role role = new Role(roleName);
                context.Roles.Add(role);
                context.SaveChanges();
                var roles = context.Roles.Select(r => new { r.Name }).ToArray();
                return roles;
            }
        }

        public List<Role> GetAllRoles()
        {
            return context.Roles.ToList();
        }

        public object AssignRole(string userName, int roleId)
        {
            UserRole userRole = new UserRole(userName, roleId);
            context.UserRoles.Add(userRole);
            context.SaveChanges();
            var a = context.Users.Where(u => u.Active == true).Except(context.UserRoles.Include(Uri => Uri.User).Select(ur => ur.User)).Select(u => new { u.UserName, User = u.UserName + ": " + u.Email }).ToArray();
            return a;
        }

        public object AddChatRoom(string roomName)
        {
            int roomCount = context.ChatRooms.Where(cr => cr.RoomName == roomName).Count();

            if (roomCount > 0)
            {
                return "Room already exists.";
            }
            else
            {
                ChatRoom chatRoom = new ChatRoom(roomName, DateTime.Now);
                context.ChatRooms.Add(chatRoom);
                context.SaveChanges();
                var chatRooms = context.ChatRooms.Select(cr => new { cr.RoomName, cr.RoomCreationDate }).ToArray();
                return chatRooms;
            }
        }

        public object AddUserToARoom(string roomName, int userRoleId, bool allowed)
        {
            int countConversationRoom = context.ConversationRooms.Where(cr => cr.RoomName == roomName && cr.UserRoleID == userRoleId).Count();
            if (countConversationRoom > 0)
            {
                return "User has already added to the room.";
            }
            else
            {
                ConversationRoom conversationRoom = new ConversationRoom(roomName, userRoleId, allowed);
                context.ConversationRooms.Add(conversationRoom);
                context.SaveChanges();
                var roomWiseUsers = context.ConversationRooms.Include(cr => cr.UserRole).Select(cr => new { cr.RoomName, cr.UserRole.UserName }).OrderBy(cr => cr.RoomName).ThenBy(cr => cr.UserName).ToArray();
                return roomWiseUsers;
            }
        }

        public void JoinRoom(string roomName, string userName)
        {
            int userRoleId = context.UserRoles.Single(ur => ur.UserName == userName).UserRoleID;
            
            Groups.Add(Context.ConnectionId, roomName);

            Connection connection = new Connection(Context.ConnectionId, userRoleId, true);
            context.Connections.Add(connection);
            context.SaveChanges();

            Clients.Caller.join(roomName);
        }

        public void Send(string room, string message, string user)
        {
            int userRoleId = context.UserRoles.Single(ur => ur.UserName == user).UserRoleID;
            ChatMessage chatMessage = new ChatMessage(userRoleId, message);
            context.ChatMessages.Add(chatMessage);
            context.SaveChanges();

            Clients.Group(room).message(room, new { message, sender = user });
        }

        public void GetByteArray(string room, string image, string user)
        {
            var token = image.Split(',');

            var token0 = token[0].Split('/');
            var token1 = token0[1].Split(';');
            string fileExtension = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + token1[0];
            string serverPath = Context.Request.GetHttpContext().Server.MapPath("/") + "Images/" + fileExtension;
            string file = serverPath.Replace("/", "\\");
            var fs = new BinaryWriter(new FileStream(file, FileMode.Append, FileAccess.Write));
            fs.Write(Convert.FromBase64String(token[1]));
            fs.Close();

            int userRoleId = context.UserRoles.Single(ur => ur.UserName == user).UserRoleID;
            ChatMessage chatMessage = new ChatMessage(userRoleId, file);
            context.ChatMessages.Add(chatMessage);
            context.SaveChanges();

            byte[] buffer = Convert.FromBase64String(token[1]);
            Clients.Group(room).image(room, new { buffer, type = token[0], sender = user });
        }

        public void GetUser(string userName)
        {
            User user = context.Users.Single(u => u.UserName == userName);
            Clients.Caller.user(new { email = user.Email, image = user.Image });
        }

        
    }
}