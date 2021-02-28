namespace ChatAppSignalRProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        ChatMessageID = c.Int(nullable: false, identity: true),
                        UserRoleID = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                        PostDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ChatMessageID)
                .ForeignKey("dbo.UserRoles", t => t.UserRoleID, cascadeDelete: true)
                .Index(t => t.UserRoleID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserRoleID = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 128),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserRoleID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserName)
                .Index(t => t.UserName)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ConnectionID = c.Int(nullable: false, identity: true),
                        ContextConnectionId = c.String(),
                        UserRoleID = c.Int(nullable: false),
                        ConnectionDateTime = c.DateTime(nullable: false),
                        Connected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ConnectionID)
                .ForeignKey("dbo.UserRoles", t => t.UserRoleID, cascadeDelete: true)
                .Index(t => t.UserRoleID);
            
            CreateTable(
                "dbo.ConversationRooms",
                c => new
                    {
                        ConversationRoomID = c.Int(nullable: false, identity: true),
                        RoomName = c.String(maxLength: 128),
                        UserRoleID = c.Int(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ConversationRoomID)
                .ForeignKey("dbo.ChatRooms", t => t.RoomName)
                .ForeignKey("dbo.UserRoles", t => t.UserRoleID, cascadeDelete: true)
                .Index(t => t.RoomName)
                .Index(t => t.UserRoleID);
            
            CreateTable(
                "dbo.ChatRooms",
                c => new
                    {
                        RoomName = c.String(nullable: false, maxLength: 128),
                        RoomCreationDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.RoomName);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false),
                        Email = c.String(),
                        Active = c.Boolean(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.UserName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "UserRoleID", "dbo.UserRoles");
            DropForeignKey("dbo.UserRoles", "UserName", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.ConversationRooms", "UserRoleID", "dbo.UserRoles");
            DropForeignKey("dbo.ConversationRooms", "RoomName", "dbo.ChatRooms");
            DropForeignKey("dbo.Connections", "UserRoleID", "dbo.UserRoles");
            DropIndex("dbo.ConversationRooms", new[] { "UserRoleID" });
            DropIndex("dbo.ConversationRooms", new[] { "RoomName" });
            DropIndex("dbo.Connections", new[] { "UserRoleID" });
            DropIndex("dbo.UserRoles", new[] { "RoleID" });
            DropIndex("dbo.UserRoles", new[] { "UserName" });
            DropIndex("dbo.ChatMessages", new[] { "UserRoleID" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.ChatRooms");
            DropTable("dbo.ConversationRooms");
            DropTable("dbo.Connections");
            DropTable("dbo.UserRoles");
            DropTable("dbo.ChatMessages");
        }
    }
}
