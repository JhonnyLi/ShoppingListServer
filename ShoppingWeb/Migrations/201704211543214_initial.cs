namespace ShoppingWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Guid(nullable: false),
                        Name = c.String(),
                        ShoppingList_ShoppingListId = c.Guid(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.ShoppingLists", t => t.ShoppingList_ShoppingListId)
                .Index(t => t.ShoppingList_ShoppingListId);
            
            CreateTable(
                "dbo.ShoppingLists",
                c => new
                    {
                        ShoppingListId = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ShoppingListId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "ShoppingList_ShoppingListId", "dbo.ShoppingLists");
            DropIndex("dbo.Items", new[] { "ShoppingList_ShoppingListId" });
            DropTable("dbo.ShoppingLists");
            DropTable("dbo.Items");
        }
    }
}
