using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatAppSignalRProject.Models
{
    public class ConversationRoom
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConversationRoomID { get; set; }

        [ForeignKey("ChatRoom")]
        public string RoomName { get; set; }

        [ForeignKey("UserRole")]
        public int UserRoleID { get; set; }
        public bool Allowed { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual UserRole UserRole { get; set; }

        public ConversationRoom()
        {
            new ChatRoom();
            new UserRole();
        }

        public ConversationRoom(string chatRoom, int userRoleId, bool allowed) : this()
        {
            RoomName = chatRoom;
            UserRoleID = userRoleId;
            Allowed = allowed;
        }
    }
}