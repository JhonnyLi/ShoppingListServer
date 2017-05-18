namespace ShoppingWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectListsToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoppingLists", "SyncIdentityUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.ShoppingLists", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "ShoppingList_ShoppingListId", c => c.Guid());
            CreateIndex("dbo.ShoppingLists", "SyncIdentityUser_Id");
            CreateIndex("dbo.ShoppingLists", "User_Id");
            CreateIndex("dbo.AspNetUsers", "ShoppingList_ShoppingListId");
            AddForeignKey("dbo.ShoppingLists", "SyncIdentityUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ShoppingLists", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "ShoppingList_ShoppingListId", "dbo.ShoppingLists", "ShoppingListId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ShoppingList_ShoppingListId", "dbo.ShoppingLists");
            DropForeignKey("dbo.ShoppingLists", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ShoppingLists", "SyncIdentityUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ShoppingList_ShoppingListId" });
            DropIndex("dbo.ShoppingLists", new[] { "User_Id" });
            DropIndex("dbo.ShoppingLists", new[] { "SyncIdentityUser_Id" });
            DropColumn("dbo.AspNetUsers", "ShoppingList_ShoppingListId");
            DropColumn("dbo.ShoppingLists", "User_Id");
            DropColumn("dbo.ShoppingLists", "SyncIdentityUser_Id");
        }
    }
}
