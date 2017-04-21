using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingWeb.Models;
using ShoppingWeb.DbOps;

namespace ShoppingWeb.Interface
{
    public class DbOperations : IDbOperations
    {
        private readonly DatabaseContext _ctx;
        public DbOperations()
        {
            _ctx = new DatabaseContext();
        }

        public List<Item> GetAllItems()
        {
            var model = _ctx.Items.OrderBy(n=>n.Name).ToList();
            return model;
        }

        public List<ShoppingList> GetAllShoppingLists()
        {
            var model = _ctx.ShoppingLists.OrderBy(n=>n.Name).ToList();
            return model;
        }

        public ShoppingList GetShoppingListByIndex(int index)
        {
            var model = _ctx.ShoppingLists.OrderBy(n => n.Name).ToList()[index];
            return model;
        }
    }
}