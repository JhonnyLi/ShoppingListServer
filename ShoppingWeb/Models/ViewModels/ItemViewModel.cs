using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWeb.Models.ViewModels
{
    public class ItemViewModel
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } //Om saken är köpt eller liknande så är den inte längre aktiv.
        public string Comment { get; set; } //Kanske läggs till i framtiden.
        public bool Deleted { get; set; }
    }
}