using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatAppSignalRProject.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatAppSignalRProject.Startup))]
namespace ChatAppSignalRProject
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CreateRolesandUsers();
            app.MapSignalR();
            
        }

        private void CreateRolesandUsers()
        {
            ChatRoomContext context = new ChatRoomContext();

            int countRole = context.Roles.Count();
            int countUser = context.Users.Count();

            if (countRole == 0 && countUser == 0)
            {
                Role adminRole = new Role();
                adminRole.Name = "Admin";
                Role userRole = new Role();
                userRole.Name = "User";
                List<Role> roles = new List<Role>();
                roles.Add(adminRole);
                roles.Add(userRole);
                context.Roles.AddRange(roles);

                User adminUser = new User();
                adminUser.UserName = "Jewel";
                adminUser.Password = "123456";
                adminUser.Email = "jewelmir81@gmail.com";
                adminUser.Active = true;
                context.Users.Add(adminUser);

                UserRole aUserRole = new UserRole();
                aUserRole.User = adminUser;
                aUserRole.Role = adminRole;
                context.UserRoles.Add(aUserRole);

                context.SaveChanges();
            }
        }
    }
}