using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatAppSignalRProject.Models
{
    public class UserRole
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleID { get; set; }

        [ForeignKey("User")]
        public string UserName { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Connection> Connections { get; set; }
        public virtual ICollection<ConversationRoom> ConversationRooms { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public UserRole()
        {
            new HashSet<Connection>();
            new HashSet<ConversationRoom>();
            new HashSet<ChatMessage>();
        }

        public UserRole(string userName, int roleId) : this()
        {
            UserName = userName;
            RoleID = roleId;
        }
    }
}