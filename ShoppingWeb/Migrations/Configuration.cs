namespace ShoppingWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ShoppingWeb.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingWeb.DbOps.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShoppingWeb.DbOps.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.People.AddOrUpdate(
            //  p => p.FullName,
            //  new Person { FullName = "Andrew Peters" },
            //  new Person { FullName = "Brice Lambson" },
            //  new Person { FullName = "Rowan Miller" }
            //);

            context.Items.AddOrUpdate(i => i.ItemId,
                new Item
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Lök"
                },
                new Item
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Gurka"
                },
                new Item
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Bröd"
                },
                new Item
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Smör"
                },
                new Item
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Gröt"
                },
                new Item
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Vattenmelon"
                },
                new Item
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Tandborste"
                }
                );

            context.ShoppingLists.AddOrUpdate(
                p => p.ShoppingListId,
                new ShoppingList
                {
                    ShoppingListId = Guid.NewGuid(),
                    Name = "Min lista"
                },
                new ShoppingList
                {
                    ShoppingListId = Guid.NewGuid(),
                    Name = "En till lista"
                }
                );

        }
    }
}
