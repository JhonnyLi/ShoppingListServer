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
            AutomaticMigrationDataLossAllowed = true;
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

            #region items
            var item1 = new Item
            {
                ItemId = Guid.NewGuid(),
                Name = "Lök"
            };

            var item2 = new Item
            {
                ItemId = Guid.NewGuid(),
                Name = "Gurka"
            };

            var item3 = new Item
            {
                ItemId = Guid.NewGuid(),
                Name = "Bröd"
            };

            var item4 = new Item
            {
                ItemId = Guid.NewGuid(),
                Name = "Smör"
            };

            var item5 = new Item
            {
                ItemId = Guid.NewGuid(),
                Name = "Gröt"
            };

            var item6 = new Item
            {
                ItemId = Guid.NewGuid(),
                Name = "Vattenmelon"
            };

            var item7 = new Item
            {
                ItemId = Guid.NewGuid(),
                Name = "Tandborste"
            };
            #endregion

            context.Items.AddOrUpdate(i => i.Name,
                item1,
                item2,
                item3,
                item4,
                item5,
                item6,
                item7  
                );

            context.ShoppingLists.AddOrUpdate(
                p => p.Name,
                new ShoppingList
                {
                    ShoppingListId = Guid.NewGuid(),
                    Name = "Min lista",
                    Items = new System.Collections.Generic.List<Item>()
                    {
                        item1,
                        item2,
                        item3
                    }
                },
                new ShoppingList
                {
                    ShoppingListId = Guid.NewGuid(),
                    Name = "En till lista",
                    Items = new System.Collections.Generic.List<Item>()
                    {
                        item4,
                        item5,
                        item6,
                        item7
                    }

                }
                );

        }
    }
}
