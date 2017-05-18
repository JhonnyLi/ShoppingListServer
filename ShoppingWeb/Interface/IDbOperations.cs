using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingWeb.Models;
using ShoppingWeb.Models.Identity;
using ShoppingWeb.Models.ViewModels;

namespace ShoppingWeb.Interface
{
    public interface IDbOperations
    {
        ShoppingList GetShoppingListByUser();
        List<ShoppingList> GetAllShoppingLists();
        List<Item> GetAllItems();

        bool AddNewList(ShoppingListViewModel model);
        bool AddOrUpdateList(ShoppingListViewModel model);
        Task<bool> AddOrUpdateListAsync(ShoppingListViewModel model);
        bool RemoveList(Guid model);
    }
}
