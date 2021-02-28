using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatAppSignalRProject.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Image { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public User()
        {
            new HashSet<UserRole>();
        }

        public User(string name, string password, string email, string image) : this()
        {
            UserName = name;
            Password = password;
            Email = email;
            Image = image;
        }
    }
}