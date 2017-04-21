﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingWeb.Models;

namespace ShoppingWeb.Interface
{
    public interface IDbOperations
    {
        ShoppingList GetShoppingListByIndex(int index);
        List<ShoppingList> GetAllShoppingLists();
        List<Item> GetAllItems();
    }
}