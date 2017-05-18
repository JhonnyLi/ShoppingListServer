using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingWeb.Models;
using ShoppingWeb.DbOps;
using ShoppingWeb.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoppingWeb.Constants;
using ShoppingWeb.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ShoppingWeb.Interface
{
    public class DbOperations : IDbOperations
    {
        //Database access
        private readonly DatabaseContext _ctx;
        private readonly string _userId;
        
        public DbOperations()
        {
            _ctx = new DatabaseContext();
            _userId = HttpContext.Current.User.Identity.GetUserId();
        }

        public bool AddNewList(ShoppingListViewModel model)
        {
            bool success = true;
            var existingShoppingList = _ctx.ShoppingLists.First(id => id.ShoppingListId == model.ShoppingListId);
            if (existingShoppingList != null)
            {
                UpdateItems(model.Items);

            }
            else
            {
                existingShoppingList = new ShoppingList()
                {
                    ShoppingListId = Guid.NewGuid(),
                    Name = model.Name,
                    Items = new List<Item>()
                };
                foreach (var item in model.Items)
                {
                    var newItem = new Item()
                    {
                        ItemId = Guid.NewGuid(),
                        Name = item.Name,
                        Active = item.Active,
                        Comment = item.Comment
                    };
                    existingShoppingList.Items.Add(newItem);
                    _ctx.Items.Add(newItem);
                }
                _ctx.ShoppingLists.Add(existingShoppingList);
            }
            try
            {
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

        private List<ItemViewModel> UpdateItems(List<ItemViewModel> items)
        {
            foreach (var item in items)
            {
                bool itemExists = _ctx.Items.Any(id => id.ItemId.ToString() == item.ItemId);
                if (itemExists)
                {
                    var itemInDatabase = _ctx.Items.First(id => id.ItemId.ToString() == item.ItemId);
                    if (item.Deleted)
                    {
                        _ctx.Items.Remove(itemInDatabase);
                    }
                    else
                    {
                        itemInDatabase.ItemId = Guid.Parse(item.ItemId);
                        itemInDatabase.Name = item.Name;
                        itemInDatabase.Active = item.Active;
                        itemInDatabase.Comment = item.Comment;
                        _ctx.Entry(itemInDatabase).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                else
                {

                    var newItem = new Item()
                    {
                        ItemId = Guid.NewGuid(),
                        Name = item.Name,
                        Active = item.Active,
                        Comment = item.Comment
                    };
                    _ctx.Items.Add(newItem);
                }

            }
            _ctx.SaveChanges();
            return items;
        }

        public List<Item> GetAllItems()
        {
            var model = _ctx.Items.OrderBy(n => n.Name).ToList();
            return model;
        }

        public List<ShoppingList> GetAllShoppingLists()
        {
            var model = _ctx.ShoppingLists.OrderBy(n => n.Name).ToList();
            return model;
        }

        public ShoppingList GetShoppingListByUser()
        {
            var model = _ctx.ShoppingLists.FirstOrDefault(id=>id.User.Id == _userId);
            return model;
        }

        public bool RemoveList(Guid model) //Denna är irrelevant i detta stadium.
        {
            throw new NotImplementedException();
        }

        public bool AddOrUpdateList(ShoppingListViewModel model)
        {
            bool success = true;
            //if (model.ListUpdated)
            //{
            //UpdateItems(model.Items);
            var existingShoppingList = _ctx.ShoppingLists.First(id => id.ShoppingListId == model.ShoppingListId) as ShoppingList;
            if (existingShoppingList != null) //Uppdatera existerande lista
            {
                existingShoppingList.Name = model.Name;
                var deletableItems = new List<ItemViewModel>();
                foreach (var item in model.Items)
                {
                    if (item.Deleted)
                    {
                        var itemToDelete = _ctx.Items.First(id => id.ItemId.ToString() == item.ItemId);
                        _ctx.Items.Remove(itemToDelete);
                        deletableItems.Add(item);
                    }
                    else
                    {
                        item.ItemId = string.IsNullOrEmpty(item.ItemId) ? Guid.Empty.ToString() : item.ItemId;
                        var itemGuid = Guid.Parse(item.ItemId);
                        if (!_ctx.Items.Any(id=>id.ItemId == itemGuid))
                        {
                            var newItem = new Item()
                            {
                                ItemId = Guid.NewGuid(),
                                Name = item.Name,
                                Active = item.Active,
                                Comment = item.Comment
                            };
                            existingShoppingList.Items.Add(newItem);
                        }else
                        {
                            var existingItem = _ctx.Items.First(id => id.ItemId.ToString() == item.ItemId);
                            existingItem.Active = item.Active;
                            existingItem.Comment = item.Comment;
                            existingItem.Name = item.Name;
                            _ctx.Entry(existingItem).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }
                model.Items.RemoveAll(n => deletableItems.Any(id => id.ItemId == n.ItemId));
                _ctx.Entry(existingShoppingList).State = System.Data.Entity.EntityState.Modified;
            }
            else //Skapa ny lista  Skiter i denna tills vidare. Bara en lista är tillgänglig.
            {
                //existingShoppingList = new ShoppingList()
                //{
                //    ShoppingListId = Guid.NewGuid(),
                //    Name = model.Name,
                //    Items = new List<Item>()
                //};
                //foreach(var item in model.Items)
                //{
                //    var newItem = new Item()
                //    {

                //    }
                //}
                //_ctx.ShoppingLists.Add(existingShoppingList);
            }
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                success = false;
                var message = ex.Message;
                throw ex;
            }
            //}
            return success;
        }

        public async Task<bool> AddOrUpdateListAsync(ShoppingListViewModel model)
        {
            bool success = true;
            if (model.ListUpdated)
            {
                UpdateItems(model.Items);
                var existingShoppingList = _ctx.ShoppingLists.First(id => id.ShoppingListId == model.ShoppingListId) as ShoppingList;
                if (existingShoppingList != null) //Uppdatera existerande lista
                {
                    existingShoppingList.Name = model.Name;
                    var deletableItems = new List<ItemViewModel>();
                    foreach (var item in model.Items)
                    {
                        if (item.Deleted)
                        {
                            var itemToDelete = _ctx.Items.First(id => id.ItemId.ToString() == item.ItemId);
                            _ctx.Items.Remove(itemToDelete);
                            deletableItems.Add(item);
                        }
                    }
                    model.Items.RemoveAll(n => deletableItems.Any(id => id.ItemId == n.ItemId));
                    _ctx.Entry(existingShoppingList).State = System.Data.Entity.EntityState.Modified;
                }
                else //Skapa ny lista  Skiter i denna tills vidare. Bara en lista är tillgänglig.
                {
                }
                try
                {
                    await _ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    success = false;
                    var message = ex.Message;
                    throw ex;
                }
            }
            return success;
        }
    }
}