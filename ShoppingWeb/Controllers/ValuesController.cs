using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShoppingWeb.Models;

namespace ShoppingWeb.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            var model = new ShoppingList();
            model.ShoppingListId = Guid.NewGuid();
            model.Name = "Min shoppinglista";
            model.Items.Add("Lök");
            model.Items.Add("Smör");
            model.Items.Add("Bröd");
            model.Items.Add("Gröt");
            model.Items.Add("Coca Cola");
            model.Items.Add("Kaviar");
            model.Items.Add("Champinjoner");
            model.Items.Add("Tomat");
            return Json(model);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
