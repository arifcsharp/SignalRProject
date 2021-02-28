using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatAppSignalRProject.Models
{
    public class ChatRoom
    {
        [Key]
        public string RoomName { get; set; }

        [DataType(DataType.Date), Column(TypeName = "DATE")]
        public DateTime RoomCreationDate { get; set; }

        public virtual ICollection<ConversationRoom> ConversationRooms { get; set; }

        public ChatRoom()
        {
            new HashSet<ConversationRoom>();
        }

        public ChatRoom(string roomName, DateTime roomCreationDate) : this()
        {
            RoomName = roomName;
            RoomCreationDate = roomCreationDate;
        }
    }
}