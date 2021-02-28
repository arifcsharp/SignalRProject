using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatAppSignalRProject.Models
{
    public class ChatMessage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatMessageID { get; set; }

        [ForeignKey("UserRole")]
        public int UserRoleID { get; set; }

        [Required]
        public string Message { get; set; }
        public DateTime PostDateTime { get; set; }

        public virtual UserRole UserRole { get; set; }

        public ChatMessage()
        {
            new UserRole();
        }

        public ChatMessage(int userRoleId, string messge) : this()
        {
            UserRoleID = userRoleId;
            Message = messge;
            PostDateTime = DateTime.Now;
        }
    }
}