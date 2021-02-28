using ChatAppSignalRProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ChatAppSignalRProject.Controllers
{
    public class HomeController : Controller
    {
        private ChatRoomContext context = new ChatRoomContext();

       
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       
        public ActionResult CreateRoom()
        {

           


            ViewBag.ChatRooms = context.ChatRooms.ToList();
            
            return View();

        }

        public ActionResult RoomAssign()
        {
            ViewBag.ChatRoomList = new SelectList(context.ChatRooms.Select(cr => new { cr.RoomName }), "RoomName", "RoomName");
            var RoomWiseUsers = context.ConversationRooms.Include(cr => cr.UserRole).OrderBy(cr => cr.RoomName).ThenBy(cr => cr.UserRole.UserName).ToList();
            ViewBag.RoomWiseUsers = RoomWiseUsers;
            ViewBag.UserListWithRole = new SelectList(context.UserRoles.Select(ur => new { ur.UserRoleID, ur.UserName }), "UserRoleID", "UserName");
            return View();
        }

        public ActionResult UserRole()
        {
            ViewBag.InactiveUserList = new SelectList(context.Users.Where(u => u.Active == false).Select(u => new { u.UserName, User = u.UserName + ": " + u.Email }), "UserName", "User");
            ViewBag.ActiveUserList = new SelectList(context.Users.Where(u => u.Active == true).Except(context.UserRoles.Include(Ur => Ur.User).Select(ur => ur.User)).Select(u => new { u.UserName, User = u.UserName + ": " + u.Email }), "UserName", "User");
            ViewBag.RoleList = new SelectList(context.Roles.Select(r => new { r.RoleID, r.Name }), "RoleID", "Name");
            ViewBag.UserListWithRole = new SelectList(context.UserRoles.Select(ur => new { ur.UserRoleID, ur.UserName }), "UserRoleID", "UserName");
            return View();
        }
        public ActionResult Role()
        {
            ViewBag.Roles = context.Roles.ToList();
            return View();
        }

        

        public ActionResult chatRoom()
        {
            return View();
        }
    }
}