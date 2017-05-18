using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWeb.Models.ViewModels
{
    public class ShoppingListViewModel
    {
        public Guid ShoppingListId { get; set; }
        public string Name { get; set; }
        public List<ItemViewModel> Items { get; set; } = new List<ItemViewModel>();
        public bool ListUpdated { get; set; } = false;

    }
}