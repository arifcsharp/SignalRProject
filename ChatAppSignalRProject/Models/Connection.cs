using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatAppSignalRProject.Models
{
    public class Connection
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConnectionID { get; set; }

        public string ContextConnectionId { get; set; }

        [ForeignKey("UserRole")]
        public int UserRoleID { get; set; }
        public DateTime ConnectionDateTime { get; set; }
        public bool Connected { get; set; }

        public virtual UserRole UserRole { get; set; }

        public Connection()
        {
            new UserRole();
        }

        public Connection(string connectionID, int userRoleId, bool connected) : this()
        {
            ContextConnectionId = connectionID;
            UserRoleID = userRoleId;
            Connected = connected;
            ConnectionDateTime = DateTime.Now;
        }
    }
    }