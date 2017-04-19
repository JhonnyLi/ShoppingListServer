using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWeb.Models
{
    public class ShoppingList
    {
        public Guid ShoppingListId { get; set; }
        public string Name { get; set; }
        public List<string> Items { get; set; } = new List<string>();
    }
}