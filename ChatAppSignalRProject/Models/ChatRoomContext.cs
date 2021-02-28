using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatAppSignalRProject.Models
{
    public class ChatRoomContext : DbContext
    {
        public ChatRoomContext() : base("ChatRoom")
        {

        }

        public static ChatRoomContext Create()
        {
            return new ChatRoomContext();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ConversationRoom> ConversationRooms { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}