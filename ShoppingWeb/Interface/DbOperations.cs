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

        public bool AddNewList(ShoppingList model)
        {
            bool success = true;
            UpdateItems(model.Items);
            try
            {
                _ctx.ShoppingLists.Add(model);
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                success = false;
                var message = ex.Message;
                throw ex;
            }
            return success;

        }

        private List<Item> UpdateItems(List<Item> items)
        {
            foreach(var item in items)
            {
                item.ItemId = Guid.NewGuid();
            }
            return items;
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

        public bool RemoveList(Guid model)
        {
            throw new NotImplementedException();
        }
    }
}