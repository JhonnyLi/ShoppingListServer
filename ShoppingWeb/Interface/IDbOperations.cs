using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingWeb.Models;
using ShoppingWeb.Models.Identity;

namespace ShoppingWeb.Interface
{
    public interface IDbOperations
    {
        ShoppingList GetShoppingListByIndex(int index);
        List<ShoppingList> GetAllShoppingLists();
        List<Item> GetAllItems();

        bool AddNewList(ShoppingList model);
        bool RemoveList(Guid model);

        List<SyncIdentityRole> GetAllRoles();
        SyncIdentityUser GetUser(string name);
        bool UserNameExists(string name);
        bool UserEmailExists(string email);

    }
}
